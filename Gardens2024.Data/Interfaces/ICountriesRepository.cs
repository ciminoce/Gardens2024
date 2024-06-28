using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ICountriesRepository
    {
        List<Country>? GetAll();
        void Add(Country country);
        void Update(Country country);
        void Delete(int id);
        Country? GetById(int id);
        bool Exist(Country country);
        bool ItsRelated(int id);

    }
}
