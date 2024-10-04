using AutoMapper;
using Garden2024.Web.Models;
using Garden2024.Web.ViewModels.Products;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList.Extensions;

namespace Garden2024.Web.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly IProductsService? _productsService;
        private readonly IMapper? _mapper;

        private readonly int pageSize = 8;
        public HomeController(IProductsService? productsService,
            IMapper? mapper)
        {
            _productsService = productsService;
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
            ViewBag.ReturnUrl=returnUrl;
            return View(productVm);
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
