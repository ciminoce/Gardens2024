using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Countries
{
    public class CountryListDto
    {
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;

    }
}
