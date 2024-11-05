using AutoMapper;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Garden2024.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderHeadersService? _orderHeadersService;
        private readonly IProductsService? _productsService;
        private readonly IMapper? _mapper;

        public OrdersController(IOrderHeadersService? orderHeadersService,
            IProductsService productService,
                IMapper? mapper)
        {
            _orderHeadersService = orderHeadersService;
            _productsService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var orderHeader = _orderHeadersService.Get(filter: o => o.OrderHeaderId == id,
                    propertiesNames: "OrderDetails");
            foreach (var item in orderHeader.OrderDetails)
            {
                var productInDetails = _productsService.Get(filter: p => p.ProductId == item.ProductId);
                item.Product = productInDetails;
            }
            return View(orderHeader);
        }

        #region API CALLS
        [HttpGet]
        [HttpGet]
        public JsonResult GetAll()
        {
            var ordersList = _orderHeadersService!.GetAll(orderBy: o => o.OrderBy(o => o.OrderHeaderId),
                propertiesNames: "ApplicationUser")!
                .Select(o => new
                {
                    o.OrderHeaderId,
                    ApplicationUser = new
                    {
                        o.ApplicationUser!.FirstName,
                        o.ApplicationUser.LastName
                    },
                    o.OrderDate,
                    o.OrderTotal
                }).ToList();
            return Json(new { data = ordersList });
        }
        #endregion

    }
}
