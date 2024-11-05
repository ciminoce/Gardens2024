using AutoMapper;
using Garden2024.Web.ViewModels.OrderHeaders;
using Garden2024.Web.ViewModels.ShoppingCarts;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Garden2024.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartsService? _cartsService;
        private readonly ICountriesService? _countriesService;
        private readonly IStatesService? _statesService;
        private readonly ICitiesService? _citiesService;
        private readonly IApplicationUsersService? _applicationUsersService;
        private readonly IOrderHeadersService? _orderHeadersService;

        private readonly IMapper? _mapper;

        public ShoppingCartsController(IShoppingCartsService? cartsService,
            ICountriesService countriesService,
            IStatesService? statesService,
            ICitiesService? citiesService,
            IApplicationUsersService? applicationUsersService,
            IOrderHeadersService? orderHeadersService,
            IMapper? mapper)
        {
            _cartsService = cartsService;
            _countriesService = countriesService ?? throw new ApplicationException("Dependencies not set");
            _statesService = statesService ?? throw new ApplicationException("Dependencies not set");
            _citiesService = citiesService ?? throw new ApplicationException("Dependencies not set");
            _applicationUsersService = applicationUsersService ?? throw new ApplicationException("Dependencies not set");
            _orderHeadersService = orderHeadersService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper;
        }

        public IActionResult Index(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var cartList = _cartsService!.GetAll(
                filter: c => c.ApplicationUserId == userId,
                propertiesNames: "Product")!.ToList();
            ShoppingCartListVm shoppingVm = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderHeader = new OrderHeaderEditVm()
                {
                    OrderTotal = CalculateTotal(cartList)

                }
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(shoppingVm);
        }

        private decimal CalculateTotal(List<ShoppingCart> cartList)
        {
            var total = 0M;
            foreach (var item in cartList)
            {
                total += (item.Quantity == 1 ? item.Product.UnitPrice : item.Product.UnitPrice * 0.9M) * item.Quantity;
            }
            return total;
        }

        public IActionResult Plus(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id, tracked: false,
                propertiesNames: "Product");


            if (cartInDb!.Product.AvailableStock >= 1)
            {
                cartInDb.Product.StockInCarts += 1;
                cartInDb!.Quantity += 1;
                cartInDb.LastUpdated = DateTime.Now;
                _cartsService.Save(cartInDb);

            }
            else
            {
                TempData["error"] = "Not enough stock available!!";
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult Minus(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id,
                propertiesNames: "Product");
            cartInDb!.Quantity -= 1;
            cartInDb.Product.StockInCarts -= 1;
            cartInDb.LastUpdated = DateTime.Now;
            if (cartInDb.Quantity == 0)
            {
                _cartsService.Delete(cartInDb);
            }
            else
            {
                _cartsService.Save(cartInDb);

            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult Remove(int id, string? returnUrl = null)
        {
            var cartInDb = _cartsService!.Get(filter: c => c.ShoppingCartId == id,
                    propertiesNames: "Product");

            cartInDb!.Product.StockInCarts -= cartInDb.Quantity;
            _cartsService.Delete(cartInDb);
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Summary(string? returnUrl = null)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "Product")!.ToList();
            ShoppingCartListVm shoppingVm = new ShoppingCartListVm
            {
                ShoppingCarts = cartList,
                OrderHeader = new OrderHeaderEditVm()
                {
                    OrderTotal = CalculateTotal(cartList),
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.AddDays(3),
                    //OrderDetails = _mapper.Map<List<OrderDetail>>(cartList),
                    Countries = GetCountries(),
                    States = GetCountryStates(),
                    Cities = GetStateCities(),

                }
            };
            var user = _applicationUsersService!.Get(filter: u => u.Id == claims!.Value);
            shoppingVm.OrderHeader.ApplicationUserId = user!.Id;
            shoppingVm.OrderHeader.FirstName = user.FirstName;
            shoppingVm.OrderHeader.LastName = user.LastName;
            shoppingVm.OrderHeader.Address = user.Address;
            shoppingVm.OrderHeader.ZipCode = user.ZipCode;
            shoppingVm.OrderHeader.CountryId = user.CountryId;
            shoppingVm.OrderHeader.StateId = user.StateId;
            shoppingVm.OrderHeader.CityId = user.CityId;
            shoppingVm.OrderHeader.Phone = user.Phone;

            ViewBag.ReturnUrl = returnUrl;

            return View(shoppingVm);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartListVm shoppingVm)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "Product")!.ToList();
            shoppingVm.OrderHeader!.OrderTotal = CalculateTotal(cartList);
            shoppingVm.OrderHeader.OrderDate = DateTime.Now;
            shoppingVm.OrderHeader.ShippingDate = DateTime.Now.AddDays(3);
            shoppingVm.OrderHeader.OrderDetails = _mapper.Map<List<OrderDetail>>(cartList);
            shoppingVm.OrderHeader.Countries = GetCountries();
            shoppingVm.OrderHeader.States = GetCountryStates();
            shoppingVm.OrderHeader.Cities = GetStateCities();
            if (!ModelState.IsValid)
            {
                return View(shoppingVm);
            }
            OrderHeader orderHeader = _mapper.Map<OrderHeader>(shoppingVm.OrderHeader);
            try
            {
                _orderHeadersService.Save(orderHeader);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(shoppingVm);
            }
            return RedirectToAction("OrderConfirmed", new { id = orderHeader.OrderHeaderId });
        }
        public IActionResult OrderConfirmed(int id)
        {
            return View(id);
        }
        public IActionResult OrderCancelled()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = _cartsService!.GetAll(
                    filter: c => c.ApplicationUserId == claims!.Value,
                    propertiesNames: "Product")!.ToList();
            foreach (var item in cartList)
            {
                item.Product.StockInCarts -= item.Quantity;
                _cartsService.Delete(item);
            }
            return View();
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
        private List<SelectListItem> GetCountryStates(int? countryId = null)
        {
            IEnumerable<State>? states;
            if (countryId is null)
            {
                states = _statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName));
            }
            else
            {
                states = _statesService!.GetAll(
                    orderBy: o => o.OrderBy(c => c.StateName),
                    filter: s => s.CountryId == countryId);

            }
            return states.Select(c => new SelectListItem
            {
                Text = c.StateName,
                Value = c.StateId.ToString()
            }).ToList();

        }
        private List<SelectListItem> GetStateCities(int? stateId = null)
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
                    filter: c => c.StateId == stateId);

            }
            return cities
                .Select(c => new SelectListItem
                {
                    Text = c.CityName,
                    Value = c.CityId.ToString()
                }).ToList();

        }
        #region API CALLS
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Json(0);

            var itemCount = _cartsService!.GetAll(filter: sc => sc.ApplicationUserId == userId)!
                                                     .Sum(sc => sc.Quantity);
            return Json(itemCount);
        }
        #endregion
    }
}
