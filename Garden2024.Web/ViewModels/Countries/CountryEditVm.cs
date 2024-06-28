using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garden2024.Web.ViewModels.Countries
{
    public class CountryEditVm
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("Country Name")]
        public string CountryName { get; set; } = null!;
    }
}
