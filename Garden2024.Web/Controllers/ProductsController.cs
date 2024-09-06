using AutoMapper;
using Garden2024.Web.ViewModels.Products;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService? _productsService;
        private readonly ICategoriesService? _categoriesService;
        private readonly IMapper? _mapper;

        public ProductsController(IProductsService? productsService,
            ICategoriesService? categoriesService,
            IMapper? mapper)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page,int? filterId,int pageSize=10, bool viewAll=false)
        {
            var currentPage=page ?? 1;
            ViewBag.currentPageSize=pageSize;
            ViewBag.currentFilterId=filterId;
            IEnumerable<Product>? products;
            if (filterId is null || viewAll)
            {
                products = _productsService?.GetAll(
                    orderBy: o => o.OrderBy(p => p.ProductName),
                    propertiesNames: "Category");
            }
            else
            {
                products = _productsService?.GetAll(
                    orderBy: o => o.OrderBy(p => p.ProductName),
                    filter:p=>p.CategoryId==filterId,
                    propertiesNames: "Category");

            }
            var productListVm = _mapper?
                .Map<List<ProductListVm>>(products);
            var productFilterVm = new ProductFilterVm()
            {
                Products = productListVm?.ToPagedList(currentPage, pageSize),
                Categories=_categoriesService!.GetAll(
                    orderBy:o=>o.OrderBy(c=>c.CategoryName))!
                .Select(c=>new SelectListItem
                {
                    Text=c.CategoryName,
                    Value=c.CategoryId.ToString()
                }).ToList()
            };
            return View(productFilterVm);
        }
    }
}
