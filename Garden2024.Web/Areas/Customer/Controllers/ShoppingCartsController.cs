using AutoMapper;
using Garden2024.Web.ViewModels.ShoppingCarts;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Garden2024.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles ="Customer")]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartsService? _cartsService;
        private readonly IMapper? _mapper;

        public ShoppingCartsController(IShoppingCartsService? cartsService, IMapper? mapper)
        {
            _cartsService = cartsService;
            _mapper = mapper;
        }

        public IActionResult Index(string? returnUrl=null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var cartList = _cartsService!.GetAll(
                filter: c => c.ApplicationUserId == userId,
                propertiesNames: "Product")!.ToList();
            ShoppingCartListVm shoppingVm = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderTotal = CalculateTotal(cartList)
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(shoppingVm);
        }

        private decimal CalculateTotal(List<ShoppingCart> cartList)
        {
            var total = 0M;
            foreach (var item in cartList)
            {
                total += item.Product.UnitPrice * item.Quantity;
            }
            return total;
        }
    }
}
