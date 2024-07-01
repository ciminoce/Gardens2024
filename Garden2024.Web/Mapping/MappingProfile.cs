using AutoMapper;
using Garden2024.Web.ViewModels.Categories;
using Garden2024.Web.ViewModels.Countries;
using Gardens2024.Entities.Entities;

namespace Garden2024.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadCategoriesMapping();
            LoadCountriesMapping();
            
        }

        private void LoadCountriesMapping()
        {
            CreateMap<Country, CountryListVm>();
            CreateMap<Country, CountryEditVm>().ReverseMap();
        }

        private void LoadCategoriesMapping()
        {
            CreateMap<Category, CategoryListVm>();
            CreateMap<Category, CategoryEditVm>().ReverseMap();
        }
    }
}
