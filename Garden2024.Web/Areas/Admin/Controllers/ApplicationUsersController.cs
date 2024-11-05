using AutoMapper;
using Garden2024.Web.ViewModels.ApplicationUsers;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Garden2024.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUsersService? _usersService;
        private readonly IMapper? _mapper;
        public ApplicationUsersController(IApplicationUsersService? usersService,
            IProductsService? productsService,
            IMapper mapper)
        {
            _usersService = usersService ?? throw new ApplicationException("Dependencies not set");
            _mapper = mapper ?? throw new ApplicationException("Dependencies not set"); ;
        }

        public IActionResult Index(int? page, string? searchTerm = null, bool viewAll = false, int pageSize = 10)
        {
            int pageNumber = page ?? 1;
            ViewBag.currentPageSize = pageSize;
            IEnumerable<ApplicationUser>? users;
            if (!viewAll)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = _usersService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                            filter: c => c.FirstName.Contains(searchTerm)
                            || c.LastName!.Contains(searchTerm),
                            propertiesNames: "Country,State,City");
                    ViewBag.currentSearchTerm = searchTerm;
                }
                else
                {
                    users = _usersService?
                            .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                        propertiesNames: "Country,State,City");
                }
            }
            else
            {
                users = _usersService?
                        .GetAll(orderBy: o => o.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
                        propertiesNames: "Country,State,City");
            }

            var usersVm = _mapper?.Map<List<ApplicationUserListVm>>(users)
                    .ToPagedList(pageNumber, pageSize);
            return View(usersVm);
        }
        public IActionResult Details(string id)
        {
            var applicationUser = _usersService.Get(filter: o => o.Id == id,
                propertiesNames: "Country,State,City,OrderHeaders");
            return View(applicationUser);
        }
    }
}
