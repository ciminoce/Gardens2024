using Garden2024.Web.ViewModels.Products;
using System.ComponentModel;
using X.PagedList;

namespace Garden2024.Web.ViewModels.Categories
{
    public class CategoryDetailsVm
    {
        public int CategoryId { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        [DisplayName("Prod. Qty")]
        public int ProductsQuantity { get; set; }

        public IPagedList<ProductListVm>? Products { get; set; }
    }
}
