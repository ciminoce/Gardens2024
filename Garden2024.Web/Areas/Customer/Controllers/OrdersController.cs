using AutoMapper;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Garden2024.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles ="Customer")]
    public class OrdersController : Controller
    {
        private readonly IOrderHeadersService? _headersService;
        private readonly IProductsService _productsService;
        private readonly IMapper? _mapper;

        public OrdersController(IOrderHeadersService? headersService,
            IProductsService productsService,
            IMapper? mapper)
        {
            _headersService = headersService;
            _productsService = productsService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var orderHeader=_headersService!.Get(filter:o=>o.OrderHeaderId== id,
                propertiesNames:"OrderDetails");
            foreach (var detail in orderHeader.OrderDetails)
            {
                var productInDetail = _productsService.Get(filter: p => p.ProductId == detail.ProductId);
                detail.Product=productInDetail;
            }
            return View(orderHeader);
        }
        #region API CALLS
        [HttpGet]
        public JsonResult GetAll()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var orderList = _headersService.GetAll(filter:
                o => o.ApplicationUserId == claims.Value);
            return Json(new {data=orderList});
        }
        #endregion
    }
}
