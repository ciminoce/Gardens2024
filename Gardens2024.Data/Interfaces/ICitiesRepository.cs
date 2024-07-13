using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ICitiesRepository : IGenericRepository<City>
    {
        bool Exist(City city);
        bool ItsRelated(int cityId);
        void Update(City city);
    }
}
