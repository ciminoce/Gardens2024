using AutoMapper;
using Garden2024.Web.ViewModels.Products;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService? _productsService;
        private readonly IMapper? _mapper;

        private readonly int pageSize = 10;
        public ProductsController(IProductsService? productsService, IMapper? mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page)
        {
            var currentPage=page ?? 1;
            var productList = _productsService?.GetAll(
                orderBy: o => o.OrderBy(p => p.ProductName),
                propertiesNames:"Category");
            var productListVm = _mapper?
                .Map<List<ProductListVm>>(productList);

            return View(productListVm?.ToPagedList(currentPage,pageSize));
        }
    }
}
