using AutoMapper;
using Garden2024.Web.ViewModels.Cities;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CitiesController : Controller
    {
        private readonly ICitiesService? _service;
        private readonly ICountriesService? _countriesService;
        private readonly IStatesService? _statesService;
        private readonly IMapper? _mapper;

        public CitiesController(ICitiesService? services,
            ICountriesService countriesService,
            IStatesService statesService,
            IMapper mapper)
        {
            _service = services;
            _countriesService = countriesService;
            _statesService = statesService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page, string? searchTerm, bool viewAll=false, int pageSize=10)
        {
            int pageNumber = page ?? 1;
            IEnumerable<City>? cities;
            ViewBag.currentPageSize = pageSize;
            if (string.IsNullOrEmpty(searchTerm) || viewAll)
            {
                cities = _service?
                    .GetAll(orderBy: q => q.OrderBy(c => c.CityName),
                    propertiesNames: "Country,State");

            }
            else
            {
                cities = _service?
                    .GetAll(orderBy: q => q.OrderBy(c => c.CityName),
                        filter:c=>c.Country.CountryName.Contains(searchTerm)
                        || c.State.StateName.Contains(searchTerm),
                    propertiesNames: "Country,State");
                ViewBag.currentSearchTerm = searchTerm;
            }

            var citiesVm = _mapper?.Map<List<CityListVm>>(cities);

            return View(citiesVm!.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult UpSert(int? id)
        {
            CityEditVm cityVm;
            if (id == null || id == 0)
            {
                cityVm = new CityEditVm();
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesService!
                    .GetAll()
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();
            }
            else
            {
                try
                {
                    City? city = _service!.Get(c => c.CityId == id.Value,
                        propertiesNames:"Country,State");
                    if (city == null)
                    {
                        return NotFound();
                    }
                    cityVm = _mapper!.Map<CityEditVm>(city);
                    cityVm.Countries = _countriesService!
                        .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        }).ToList();
                    cityVm.States = _statesService!
                        .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                        .Select(s => new SelectListItem
                        {
                            Text = s.StateName,
                            Value = s.StateId.ToString()
                        }).ToList();

                    return View(cityVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            cityVm.Countries = _countriesService
                .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                .Select(c => new SelectListItem
                {
                    Text = c.CountryName,
                    Value = c.CountryId.ToString()
                }).ToList();
            cityVm.States = _statesService
                .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                .Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString()
                }).ToList();

            return View(cityVm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(CityEditVm cityVm)
        {
            if (!ModelState.IsValid)
            {
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesService!
                    .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();

                return View(cityVm);
            }


            try
            {
                City City = _mapper!.Map<City>(cityVm);

                if (_service!.Exist(City))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    cityVm.Countries = _countriesService!
                        .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        }).ToList();
                    cityVm.States = _statesService!
                        .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                        .Select(s => new SelectListItem
                        {
                            Text = s.StateName,
                            Value = s.StateId.ToString()
                        }).ToList();

                    return View(cityVm);
                }

                _service.Save(City);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                cityVm.Countries = _countriesService!
                    .GetAll(orderBy: q => q.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    }).ToList();
                cityVm.States = _statesService!
                    .GetAll(filter: s => s.CountryId == cityVm.CountryId)
                    .Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();

                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                return View(cityVm);
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
            City? city = _service?.Get(filter: c => c.CityId == id.Value);
            if (city is null)
            {
                return NotFound();
            }
            try
            {

                if (_service!.ItsRelated(city.CityId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _service.Remove(city);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }


        public JsonResult GetStates(int countryId)
        {
            var states = _statesService?
                .GetAll(filter: s => s.CountryId == countryId,
                orderBy: q => q.OrderBy(c => c.StateName));
            return Json(states);
        }

    }

}
