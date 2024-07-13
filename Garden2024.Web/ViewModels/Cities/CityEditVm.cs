using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Garden2024.Web.ViewModels.Cities
{
    public class CityEditVm
    {
        public int CityId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(200, ErrorMessage = "{0} must have between {2} and {1} characters", MinimumLength = 3)]
        [DisplayName("City Name")]

        public string CityName { get; set; } = null!;
        [Required(ErrorMessage = "Country is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        [DisplayName("Country")]

        public int CountryId { get; set; }
        [Required(ErrorMessage = "State is required")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a state")]
        [DisplayName("State")]

        public int StateId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; }=null!;
        [ValidateNever]

        public IEnumerable<SelectListItem> States { get; set; }=null!;

    }
}
