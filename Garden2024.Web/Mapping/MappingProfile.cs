using AutoMapper;
using Garden2024.Web.ViewModels.Categories;
using Garden2024.Web.ViewModels.Cities;
using Garden2024.Web.ViewModels.Countries;
using Garden2024.Web.ViewModels.States;
using Gardens2024.Entities.Entities;

namespace Garden2024.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadCategoriesMapping();
            LoadCountriesMapping();
            LoadStatesMapping();
            LoadCitiesMapping();
        }

        private void LoadCitiesMapping()
        {
            CreateMap<City, CityListVm>().
                ForMember(dest => dest.CountryName,
                opt => opt.MapFrom(c => c.Country.CountryName))
                .ForMember(dest => dest.StateName,
                opt => opt.MapFrom(s => s.State.StateName));
            CreateMap<City, CityEditVm>();
        }

        private void LoadStatesMapping()
        {
            CreateMap<State, StateListVm>()
                .ForMember(dest => dest.Country,
                opt => opt.MapFrom(src => src.Country.CountryName));
            CreateMap<State, StateEditVm>().ReverseMap();
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
