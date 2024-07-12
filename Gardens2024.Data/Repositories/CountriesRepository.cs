using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly Gardens2024DbContext _db;

        public CountriesRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }



        public bool Exist(Country country)
        {
            if (country.CountryId == 0)
            {
                return _db.Countries.Any(c => c.CountryName == country.CountryName);
            }
            return _db.Countries.Any(c => c.CountryName == country.CountryName && c.CountryId != country.CountryId);
        }


        public bool ItsRelated(int id)
        {
            return _db.States.Any(p => p.CountryId == id);
        }

        public void Update(Country country)
        {
            _db.Countries.Update(country);

        }

    }
}
