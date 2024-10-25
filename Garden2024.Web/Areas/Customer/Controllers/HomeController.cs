using AutoMapper;
using Garden2024.Web.Models;
using Garden2024.Web.ViewModels.Products;
using Garden2024.Web.ViewModels.ShoppingCarts;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace Garden2024.Web.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly IProductsService? _productsService;
        private readonly IShoppingCartsService _shoppingCartService;
        private readonly IMapper? _mapper;

        private readonly int pageSize = 8;
        public HomeController(IProductsService? productsService,
            IShoppingCartsService shoppingCartsService,
            IMapper? mapper)
        {
            _productsService = productsService;
            _shoppingCartService = shoppingCartsService;
            _mapper = mapper;
        }

        public IActionResult Hero()
        {
            return View();
        }
        public IActionResult Index(int? page)
        {
            var currentPage = page ?? 1;
            var products = _productsService!.GetAll(
                orderBy: o => o.OrderBy(p => p.ProductName),
                propertiesNames: "Category");
            var productsVm = _mapper!.Map<List<ProductHomeIndexVm>>(products);
            ViewBag.CurrentPage=currentPage;
            return View(productsVm.ToPagedList(currentPage,pageSize));
        }
        public IActionResult Details(int? id, string? returnUrl=null)
        {
            if (id == null || id.Value == 0) { return NotFound(); }
            Product? product = _productsService!.Get(
                filter: p => p.ProductId == id,
                propertiesNames: "Category,Supplier");
            if (product is null)
            {
                return NotFound();
            }
            ProductHomeDetailsVm productVm = _mapper!.Map<ProductHomeDetailsVm>(product);
            ShoppingCartDetailVm shoppingVm = new ShoppingCartDetailVm
            {
                ProductId = product.ProductId,
                Product = productVm,
                Quantity = 1
            };
            ViewBag.ReturnUrl=returnUrl;
            return View(shoppingVm);
        }

        [HttpPost]
        [Authorize(Roles ="Customer")]
        public IActionResult Details(ShoppingCartDetailVm shoppingVm,string? returnUrl=null)
        {
            ClaimsIdentity claimsIdentity =(ClaimsIdentity) User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shoppingVm.ApplicationUserId=userId;

            ShoppingCart shoppingCart = _mapper!.Map<ShoppingCart>(shoppingVm);
            var cartInDb=_shoppingCartService.Get(filter:s=>s.ProductId==shoppingCart.ProductId &&
                    s.ApplicationUserId==shoppingCart.ApplicationUserId);
            var product = _productsService?.Get(filter: p => p.ProductId == shoppingVm.ProductId);

            if (product.AvailableStock>=shoppingCart.Quantity)
            {
                if (cartInDb == null)
                {
                    product.StockInCarts += shoppingCart.Quantity;
                    shoppingCart.Product = product;
                    _shoppingCartService.Save(shoppingCart);
                }
                else
                {
                    product.StockInCarts += shoppingCart.Quantity;
                    cartInDb.Quantity += shoppingCart.Quantity;
                    shoppingCart.Product = product;
                    _shoppingCartService.Save(cartInDb);
                }
                TempData["success"] = "Product successfully added to shopping cart";

            }
            else
            {
                TempData["error"] = "Not enough stock!!";

            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
