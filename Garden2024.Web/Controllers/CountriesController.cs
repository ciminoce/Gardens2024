using AutoMapper;
using Garden2024.Web.ViewModels.Countries;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountriesService? _countriesService;
        private readonly IMapper? _mapper;

        private int pageSize = 10;
        public CountriesController(ICountriesService? countriesService,
            IMapper? mapper)
        {
            _countriesService = countriesService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page = null)
        {
            int pageNumber = page ?? 1;
            var countries = _countriesService?
                .GetAll(orderBy:o=>o.OrderBy(c=>c.CountryName));
            var countriesVm = _mapper?.Map<List<CountryListVm>>(countries)
                .ToPagedList(pageNumber, pageSize);

            return View(countriesVm);
        }

        public IActionResult UpSert(int? id)
        {
            if (_countriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }
            CountryEditVm countryVm;
            if (id == null || id == 0)
            {
                countryVm = new CountryEditVm();
            }
            else
            {
                try
                {
                    Country? country = _countriesService.Get(filter:c=>c.CountryId==id);
                    if (country == null)
                    {
                        return NotFound();
                    }
                    countryVm = _mapper.Map<CountryEditVm>(country);
                    return View(countryVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(countryVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(CountryEditVm countryVm)
        {
            if (!ModelState.IsValid)
            {
                return View(countryVm);
            }

            if (_countriesService == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                Country Country = _mapper.Map<Country>(countryVm);

                if (_countriesService.Exist(Country))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    return View(countryVm);
                }

                _countriesService.Save(Country);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(countryVm);
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
            Country? country = _countriesService?.Get(filter:c=>c.CountryId==id);
            if (country is null)
            {
                return NotFound();
            }
            try
            {
                if (_countriesService == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_countriesService.ItsRelated(country.CountryId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _countriesService.Delete(country);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }

    }
}
