using AutoMapper;
using Garden2024.Web.ViewModels.Categories;
using Gardens2024.Entities.Entities;

namespace Garden2024.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryEditVm>().ReverseMap();
            
        }
    }
}
