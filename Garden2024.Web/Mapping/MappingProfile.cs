using AutoMapper;
using Garden2024.Web.ViewModels.ApplicationUsers;
using Garden2024.Web.ViewModels.Categories;
using Garden2024.Web.ViewModels.Cities;
using Garden2024.Web.ViewModels.Countries;
using Garden2024.Web.ViewModels.Products;
using Garden2024.Web.ViewModels.ShoppingCarts;
using Garden2024.Web.ViewModels.States;
using Garden2024.Web.ViewModels.Suppliers;
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
            LoadSuppliersMapping();
            LoadProductsMapping();
            LoadApplicationUsersMapping();
            LoadShoppingCartsMapping();
        }

        private void LoadShoppingCartsMapping()
        {
            CreateMap<ShoppingCartDetailVm, ShoppingCart>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.ApplicationUserId));

        }

        private void LoadApplicationUsersMapping()
        {
            CreateMap<ApplicationUser,ApplicationUserListVm>()
                 .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName));

        }

        private void LoadProductsMapping()
        {
            CreateMap<Product, ProductListVm>()
                .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<Product, ProductEditVm>().ReverseMap();
            CreateMap<Product, ProductHomeIndexVm>()
                .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.CashPrice, opt => opt.MapFrom(src => src.UnitPrice * 0.9m));
            CreateMap<Product, ProductHomeDetailsVm>()
                .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.CashPrice, opt => opt.MapFrom(src => src.UnitPrice * 0.9m))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.SupplierName));

        }

        private void LoadSuppliersMapping()
        {
            CreateMap<Supplier, SupplierListVm>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName));
            CreateMap<Supplier, SupplierEditVm>().ReverseMap();
            CreateMap<Supplier, SupplierDetailsVm>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.CityName))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.StateName))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName));

        }

        private void LoadCitiesMapping()
        {
            CreateMap<City, CityListVm>().
                ForMember(dest => dest.CountryName,
                opt => opt.MapFrom(c => c.Country.CountryName))
                .ForMember(dest => dest.StateName,
                opt => opt.MapFrom(s => s.State.StateName));
            CreateMap<City, CityEditVm>().ReverseMap();
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
            CreateMap<Category, CategoryDetailsVm>();
            CreateMap<Category, CategoryEditVm>().ReverseMap();
        }
    }
}
