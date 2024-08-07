﻿using AutoMapper;
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
            var categories = _categoriesService?
                .GetAll(orderBy:o=>o.OrderBy(c=>c.CategoryName));
            var categoriesVm = _mapper?.Map<List<CategoryListVm>>(categories)
                .ToPagedList(pageNumber, pageSize);
                
            return View(categoriesVm);
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

    }
}
