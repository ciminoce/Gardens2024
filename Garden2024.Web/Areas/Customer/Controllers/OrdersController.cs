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
        private readonly IMapper? _mapper;

        public OrdersController(IOrderHeadersService? headersService, IMapper? mapper)
        {
            _headersService = headersService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
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
