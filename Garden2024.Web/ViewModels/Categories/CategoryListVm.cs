using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Categories
{
    public class CategoryListVm
    {
        public int CategoryId { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }

    }
}
