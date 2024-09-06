using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace Garden2024.Web.ViewModels.Products
{
    public class ProductFilterVm
    {
        public IPagedList<ProductListVm>? Products { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
