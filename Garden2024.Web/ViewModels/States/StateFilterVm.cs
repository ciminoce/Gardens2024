using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace Garden2024.Web.ViewModels.States
{
    public class StateFilterVm
    {
        public IPagedList<StateListVm>? States { get; set; }
        public List<SelectListItem>? Countries { get; set; }
    }
}
