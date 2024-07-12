using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ICountriesRepository:IGenericRepository<Country>
    {
        void Update(Country country);
        bool Exist(Country country);
        bool ItsRelated(int id);

    }
}
