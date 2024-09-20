using AutoMapper;
using Garden2024.Web.ViewModels.Categories;
using Garden2024.Web.ViewModels.Products;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService? _categoriesService;
        private readonly IProductsService? _productsService;
        private readonly IMapper? _mapper;
        public CategoriesController(ICategoriesService? categoriesService,
            IProductsService? productsService,
            IMapper mapper)
        {
            _categoriesService = categoriesService??throw new ApplicationException("Dependencies not set");
            _productsService = productsService ?? throw new ApplicationException("Dependencies not set"); ;
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set"); ;
        }

        public IActionResult Index(int? page, string? searchTerm=null, bool viewAll=false, int pageSize=10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize=pageSize;
            IEnumerable<Category>? categories;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    categories = _categoriesService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.CategoryName),
                            filter: c => c.CategoryName.Contains(searchTerm)
                            || c.Description!.Contains(searchTerm));
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    categories = _categoriesService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.CategoryName));
                }

            }
            else
            {
                categories = _categoriesService?
                    .GetAll(orderBy: o => o.OrderBy(c => c.CategoryName));

            }
            var categoriesVm = _mapper?.Map<List<CategoryListVm>>(categories)
                .ToPagedList(pageNumber, pageSize);

            foreach (var category in categoriesVm!)
            {
                category.ProductsQuantity = GetProductQuantity(category.CategoryId);
            }    
            return View(categoriesVm);
        }
        private int GetProductQuantity(int categoryId)
        {
            return _productsService!.GetAll(
                    filter: p => p.CategoryId == categoryId)!.Count();
        }
        public IActionResult UpSert(int? id)
        {
            if (_categoriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            CategoryEditVm categoryVm;
            if (id == null || id == 0)
            {
                categoryVm = new CategoryEditVm();
            }
            else
            {
                try
                {
                    Category? category = _categoriesService.Get(filter:c=>c.CategoryId==id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    categoryVm = _mapper.Map<CategoryEditVm>(category);
                    return View(categoryVm);
                }
                catch (Exception )
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(categoryVm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(CategoryEditVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryVm);
            }

            if (_categoriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Category category = _mapper.Map<Category>(categoryVm);

                if (_categoriesService.Exist(category))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(categoryVm);
                }

                _categoriesService.Save(category);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(categoryVm);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id==0)
            {
                return NotFound();
            }
            Category? category=_categoriesService?.Get(filter: c => c.CategoryId == id);
            if (category is null)
            {
                return NotFound();
            }
            try
            {
                if (_categoriesService == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_categoriesService.ItsRelated(category.CategoryId))
                {
                    return Json(new { success = false, message="Related Record... Delete Deny!!" }); ;
                }
                _categoriesService.Delete(category);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }

        public IActionResult Details(int? id,int? page)
        {

            if (id is null || id == 0)
            {
                return NotFound();
            }
            Category? category = _categoriesService?.Get(filter: c => c.CategoryId == id);
            if (category is null)
            {
                return NotFound();
            }
            var currentPage=page?? 1;
            int pageSize = 10;
            CategoryDetailsVm categoryVm = _mapper!.Map<CategoryDetailsVm>(category);
            categoryVm.ProductsQuantity = GetProductQuantity(categoryVm.CategoryId);
            var products = _productsService!.GetAll(
                orderBy: o => o.OrderBy(p => p.ProductName),
                filter: p => p.CategoryId == categoryVm.CategoryId,
                propertiesNames: "Category");
            categoryVm.Products = _mapper!.Map<List<ProductListVm>>(products).ToPagedList(currentPage, pageSize);
            return View(categoryVm);
        }
    }
}
