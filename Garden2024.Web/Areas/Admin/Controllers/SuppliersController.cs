using AutoMapper;
using Garden2024.Web.ViewModels.Suppliers;
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

    public class SuppliersController : Controller
    {
        private readonly ISuppliersService? _services;
        private readonly ICountriesService? _countriesService;
        private readonly IStatesService? _statesService;
        private readonly ICitiesService? _citiesService;
        private readonly IMapper? _mapper;

        private int pageSize = 10;

        public SuppliersController(ISuppliersService? services,
            ICountriesService countriesService,
            IStatesService statesService,
            ICitiesService citiesService,
            IMapper? mapper)
        {
            _services = services ?? throw new ApplicationException("Dependencies not set");
            _countriesService = countriesService ?? throw new ApplicationException("Dependencies not set");
            _statesService = statesService ?? throw new ApplicationException("Dependencies not set");
            _citiesService = citiesService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set");
        }

        public IActionResult Index(int? page)
        {
            var currentPage = page ?? 1;
            var supplierList = _services?
                    .GetAll(
                        orderBy: o => o.OrderBy(s => s.SupplierName),
                        propertiesNames: "Country,State,City");
            var supplierListVm = _mapper?.Map<List<SupplierListVm>>(supplierList);
            return View(supplierListVm?.ToPagedList(currentPage, pageSize));
        }

        public IActionResult UpSert(int? id)
        {
            SupplierEditVm supplierVm;
            if (id == null || id == 0)
            {
                supplierVm = new SupplierEditVm();
                supplierVm.Countries =GetCountries();
                supplierVm.States = GetCountryStates();
                supplierVm.Cities = GetStateCities();


            }
            else
            {
                try
                {
                    Supplier? supplier = _services!.Get(filter: c => c.SupplierId == id);
                    if (supplier == null)
                    {
                        return NotFound();
                    }
                    supplierVm = _mapper!.Map<SupplierEditVm>(supplier);
                    supplierVm.Countries = GetCountries();
                    supplierVm.States = GetCountryStates(supplier.CountryId);
                    supplierVm.Cities = GetStateCities(supplier.StateId);


                    return View(supplierVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(supplierVm);

        }

        private List<SelectListItem> GetCountries()
        {
            return _countriesService!.GetAll(
                                    orderBy: o => o.OrderBy(c => c.CountryName))
                                .Select(c => new SelectListItem
                                {
                                    Text = c.CountryName,
                                    Value = c.CountryId.ToString()
                                }).ToList();
        }
        private List<SelectListItem> GetCountryStates(int? countryId=null)
        {
            IEnumerable<State>? states;
            if (countryId is null)
            {
                states = _statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName));
            }
            else
            {
                states=_statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName),
                    filter: s => s.CountryId == countryId);

            }
            return states.Select(c => new SelectListItem
                   {
                       Text = c.StateName,
                       Value = c.StateId.ToString()
                   }).ToList();

        }
        private List<SelectListItem> GetStateCities(int? stateId=null)
        {
            IEnumerable<City>? cities;
            if (stateId is null)
            {
                cities = _citiesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.CityName));
            }
            else
            {
                cities = _citiesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.CityName),
                    filter:c=>c.StateId==stateId);

            }
            return cities
                .Select(c => new SelectListItem
                {
                    Text = c.CityName,
                    Value = c.CityId.ToString()
                }).ToList();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(SupplierEditVm supplierVm)
        {
            if (!ModelState.IsValid)
            {
                supplierVm.Countries = GetCountries();
                supplierVm.States = GetCountryStates(supplierVm.CountryId);
                supplierVm.Cities = GetStateCities(supplierVm.StateId);

                return View(supplierVm);
            }


            try
            {
                Supplier supplier = _mapper!.Map<Supplier>(supplierVm);

                if (_services!.Exist(supplier))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    supplierVm.Countries = GetCountries();
                    supplierVm.States = GetCountryStates(supplier.CountryId);
                    supplierVm.Cities = GetStateCities(supplier.StateId);

                    return View(supplierVm);
                }

                _services.Save(supplier);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                supplierVm.Countries = GetCountries();
                supplierVm.States = GetCountryStates(supplierVm.CountryId);
                supplierVm.Cities = GetStateCities(supplierVm.StateId);

                return View(supplierVm);
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
            Supplier? supplier = _services?.Get(filter: c => c.SupplierId == id);
            if (supplier is null)
            {
                return NotFound();
            }
            try
            {
                if (_services!.ItsRelated(supplier.SupplierId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _services.Delete(supplier);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }
        public JsonResult GetStates(int countryId)
        {
            var stateList = _statesService?
                .GetAll(filter: s => s.CountryId == countryId);
            return Json(stateList);
        }
        public JsonResult GetCities(int stateId)
        {
            var citiesList = _citiesService?
                .GetAll(filter: s => s.StateId == stateId);
            return Json(citiesList);
        }
        public IActionResult Details(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                if (_services == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }
                Supplier? supplier = _services?.Get(filter: c => c.SupplierId == id.Value,
                            propertiesNames: "Country,State,City");
                if (supplier is null)
                {
                    return NotFound();
                }
                SupplierDetailsVm supplierVm = _mapper.Map<SupplierDetailsVm>(supplier);
                return View(supplierVm);
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }

        }

    }
}
