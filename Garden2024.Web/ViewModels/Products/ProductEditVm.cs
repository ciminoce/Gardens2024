using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Products
{
    public class ProductEditVm
    {
        public int ProductId { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [StringLength(255, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Product Name")]

        public string ProductName { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "{0} must have no more than {1} characters")]
        [DisplayName("Latin Name")]

        public string? LatinName { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a supplier")]
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a category")]
        [DisplayName("Category")]

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Qty. Per Unit")]

        public string? QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue)]
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(1, int.MaxValue)]
        [DisplayName("Stock")]
        public double Stock { get; set; }

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }  // Propiedad para la imagen

        [Display(Name = "Remove Image")]
        public bool RemoveImage { get; set; }  // Propiedad para borrar imagen cargada
        public bool Suspended { get; set; }
        [ValidateNever]
        public List<SelectListItem>? Categories { get; set; }
        [ValidateNever]
        public List<SelectListItem>? Suppliers { get; set; }


    }
}
