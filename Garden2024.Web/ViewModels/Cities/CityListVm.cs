using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Cities
{
    public class CityListVm
    {
        public int CityId { get; set; }
        [DisplayName("City")]
        public string? CityName { get; set; }
        [DisplayName("State")]

        public string? StateName { get; set; }
        [DisplayName("Country")]

        public string? CountryName { get; set; }
    }
}
