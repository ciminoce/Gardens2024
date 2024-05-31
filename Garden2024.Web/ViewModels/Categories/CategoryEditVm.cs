using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garden2024.Web.ViewModels.Categories
{
    public class CategoryEditVm
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="{0} is required")]
        [StringLength(200,ErrorMessage ="{0} must have between {2} and {1} characters", MinimumLength =3)]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; } = null!;
        [MaxLength(256,ErrorMessage ="{0} must have less than 257 characters")]
        public string? Description { get; set; }
    }
}
