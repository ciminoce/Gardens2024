using Gardens2024.Entities.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Garden2024.Web.ViewModels.OrderHeaders
{
    public class OrderHeaderEditVm
    {
        public int OrderHeaderId { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ApplicationUser? ApplicationUser { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Address { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "{0} must have at least {1} characters")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; } = null!;

        [Required(ErrorMessage = "Country is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a state")]
        [DisplayName("State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        [DisplayName("City")]
        public int CityId { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> States { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> Cities { get; set; } = null!;

    }
}
