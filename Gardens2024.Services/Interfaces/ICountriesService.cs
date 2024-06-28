using Gardens2024.Entities.Entities;

namespace Gardens2024.Services.Interfaces
{
    public interface ICountriesService
    {
        List<Country>? GetAll();
        void Save(Country country);
        void Delete(int id);
        Country? GetById(int id);
        bool Exist(Country country);
        bool ItsRelated(int id);

    }
}
