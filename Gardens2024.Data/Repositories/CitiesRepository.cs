using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly Gardens2024DbContext _context;
        public CitiesRepository(Gardens2024DbContext context) : base(context)
        {
            _context = context;
        }

        public bool Exist(City city)
        {
            return city.CityId == 0 ? _context.Cities.Any(c => c.CityName == city.CityName
                    && c.StateId == city.StateId
                    && c.CountryId == city.CountryId) : _context.Cities.Any(c => c.CityName == city.CityName
                    && c.StateId == city.StateId
                    && c.CountryId == city.CountryId
                    && c.CityId != city.CityId);

        }

        public bool ItsRelated(int cityId)
        {
            return false;
        }

        public void Update(City city)
        {
            _context.Update(city);
        }
    }
}
