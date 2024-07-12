using AutoMapper;
using Garden2024.Web.ViewModels.States;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace Garden2024.Web.Controllers
{
    public class StatesController : Controller
    {
        private readonly IStatesService? _service;
        private readonly ICountriesService? _countriesService;
        private readonly IMapper? _mapper;

        private readonly int pageSize = 10;

        public StatesController(IStatesService? service,
            ICountriesService countriesService,
            IMapper? mapper)
        {
            _service = service ?? throw new ArgumentException("Dependencies not set");
            _countriesService = countriesService ?? throw new ArgumentException("Dependencies not set"); ;
            _mapper = mapper ?? throw new ArgumentException("Dependencies not set"); ;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var states = _service!
                .GetAll(orderBy: o => o.OrderBy(s => s.StateName),
                propertiesNames: "Country");
            var statesVm = _mapper!.Map<List<StateListVm>>(states);
            return View(statesVm.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult UpSert(int? id)
        {
            StateEditVm stateVm;
            if (id == null || id == 0)
            {
                stateVm = new StateEditVm();
                stateVm.Countries =
                    _countriesService!
                    .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    })
                    .ToList();
            }
            else
            {
                try
                {
                    State? state = _service!.Get(filter: c => c.StateId == id);
                    if (state == null)
                    {
                        return NotFound();
                    }
                    stateVm = _mapper!.Map<StateEditVm>(state);
                    stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                    return View(stateVm);
                }
                catch (Exception)
                {
                    // Log the exception (ex) here as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the record.");
                }

            }
            return View(stateVm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(StateEditVm stateVm)
        {
            if (!ModelState.IsValid)
            {
                stateVm.Countries =
                    _countriesService!
                    .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                    .Select(c => new SelectListItem
                    {
                        Text = c.CountryName,
                        Value = c.CountryId.ToString()
                    })
                    .ToList();

                return View(stateVm);
            }

            if (_service == null || _mapper == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
            }

            try
            {
                State state = _mapper.Map<State>(stateVm);

                if (_service.Exist(state))
                {
                    ModelState.AddModelError(string.Empty, "Record already exist");
                    stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                    return View(stateVm);
                }

                _service.Save(state);
                TempData["success"] = "Record successfully added/edited";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                ModelState.AddModelError(string.Empty, "An error occurred while editing the record.");
                stateVm.Countries =
                        _countriesService!
                        .GetAll(orderBy: o => o.OrderBy(c => c.CountryName))
                        .Select(c => new SelectListItem
                        {
                            Text = c.CountryName,
                            Value = c.CountryId.ToString()
                        })
                        .ToList();

                return View(stateVm);
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
            State? state = _service?.Get(filter: c => c.StateId == id);
            if (state is null)
            {
                return NotFound();
            }
            try
            {
                if (_service == null || _mapper == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dependencias no están configuradas correctamente");
                }

                if (_service.ItsRelated(state.StateId))
                {
                    return Json(new { success = false, message = "Related Record... Delete Deny!!" }); ;
                }
                _service.Delete(state);
                return Json(new { success = true, message = "Record successfully deleted" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Couldn't delete record!!! " }); ;

            }
        }
    }
}
