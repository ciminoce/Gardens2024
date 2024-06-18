using AutoMapper;
using Garden2024.Web.ViewModels.Categories;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Garden2024.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService? _categoriesService;
        private readonly IMapper? _mapper;
        public CategoriesController(ICategoriesService? categoriesService,
            IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var categories = _categoriesService?.GetAll()
                .ToPagedList(pageNumber, pageSize);
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryEditVm categoryVm)
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
                    ModelState.AddModelError(string.Empty, "Registro duplicado!!!");
                    return View(categoryVm);
                }

                _categoriesService.Save(category);
                TempData["success"] = "Register successfully added";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while creating the record.");
                return View(categoryVm);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_categoriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Category? category = _categoriesService.GetById(id.Value);
                if (category == null)
                {
                    return NotFound();
                }
                CategoryEditVm categoryVm = _mapper.Map<CategoryEditVm>(category);
                return View(categoryVm);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
            }
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditVm categoryVm)
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
                TempData["success"] = "Record successfully edited";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(categoryVm);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (_categoriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Category? category = _categoriesService.GetById(id.Value);
                if (category == null)
                {
                    return Json(new { success = false });
                }

                _categoriesService.Delete(category.CategoryId);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                return Json(new { success = false, message = "An error occurred while deleting the record." });
            }
        }

    }
}
