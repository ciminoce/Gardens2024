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
        private readonly ISuppliersService? _suppliersService;
        private readonly IMapper? _mapper;
        private readonly IWebHostEnvironment? _webHostEnvironment;

        public ProductsController(IProductsService? productsService,
            ICategoriesService? categoriesService,
            ISuppliersService suppliersService,
            IWebHostEnvironment webHostEnvironment,
            IMapper? mapper)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _suppliersService = suppliersService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public IActionResult Index(int? page, int? filterId, int pageSize = 10, bool viewAll = false)
        {
            var currentPage = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            ViewBag.currentFilterId = filterId;
            IEnumerable<Product>? products;
            if (filterId is null || viewAll)
            {
                products = _productsService?.GetAll(
                    orderBy: o => o.OrderBy(p => p.ProductName),
                    propertiesNames: "Category,Supplier");
            }
            else
            {
                products = _productsService?.GetAll(
                    orderBy: o => o.OrderBy(p => p.ProductName),
                    filter: p => p.ProductId == filterId,
                    propertiesNames: "Category,Supplier");

            }
            var productListVm = _mapper?
                .Map<List<ProductListVm>>(products);
            var productFilterVm = new ProductFilterVm()
            {
                Products = productListVm?.ToPagedList(currentPage, pageSize),
                Categories = GetCategories()
            };
            return View(productFilterVm);
        }

        private List<SelectListItem> GetCategories()
        {
            return _categoriesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.CategoryName))!
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList();
        }

        public IActionResult UpSert(int? id)
        {
            ProductEditVm productVm;
            if (id == null || id == 0)
            {
                productVm = new ProductEditVm();
                productVm.Categories = GetCategories();
                productVm.Suppliers = GetSuppliers();
            }
            else
            {
                try
                {
                    Product? product = _productsService!.Get(filter: c => c.ProductId == id,
                        propertiesNames: "Category,Supplier");
                    if (product == null)
                    {
                        return NotFound();
                    }
                    productVm = _mapper!.Map<ProductEditVm>(product);
                    productVm.Categories = GetCategories();
                    productVm.Suppliers = GetSuppliers();
                    return View(productVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(productVm);

        }

        private List<SelectListItem>? GetSuppliers()
        {
            return _suppliersService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.SupplierName))!
                .Select(c => new SelectListItem
                {
                    Text = c.SupplierName,
                    Value = c.SupplierId.ToString()
                }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductEditVm productVm)
        {
            if (!ModelState.IsValid)
            {
                productVm.Categories = GetCategories();
                productVm.Suppliers = GetSuppliers();

                return View(productVm);
            }

            if (_categoriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Product product = _mapper.Map<Product>(productVm);

                if (_productsService!.Exist(product))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    productVm.Categories = GetCategories();
                    productVm.Suppliers = GetSuppliers();

                    return View(productVm);
                }
                if (productVm.ImageFile != null)
                {
                    string? wwwWebRoot = _webHostEnvironment!.WebRootPath;

                    string fileName=$"{Guid.NewGuid()}{Path.GetExtension(productVm.ImageFile.FileName)}";
                    string pathName=Path.Combine(wwwWebRoot,"images", fileName);

                    using (var fileStream = new FileStream(pathName, FileMode.Create))
                    {
                        productVm.ImageFile.CopyTo(fileStream);
                    }
                    product.ImageUrl=$"/images/{fileName}";
                }
                _productsService.Save(product);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                productVm.Categories = GetCategories();
                productVm.Suppliers = GetSuppliers();

                return View(productVm);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            Product? product = _productsService?.Get(filter: c => c.ProductId == id,
                propertiesNames:"Category,Supplier");
            if (product is null)
            {
                return NotFound();
            }
            try
            {
                if (_categoriesService == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_productsService!.ItsRelated(product.ProductId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _productsService.Delete(product);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }


    }
}
