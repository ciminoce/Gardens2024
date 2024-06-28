using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gardens2024.Data.Repositories
{
    public class CountriesRepository:ICountriesRepository
    {
        private readonly Gardens2024DbContext _context;

        public CountriesRepository(Gardens2024DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            _context.Countries.Add(country);

        }

        public void Delete(int id)
        {
            var countryInDb = GetById(id);
            if (countryInDb != null)
            {
                _context.Countries.Remove(countryInDb);

            }
            else
            {
                throw new Exception("Record deleted by other user!!!");
            }
        }

        public bool Exist(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (country.CountryId == 0)
            {
                return _context.Countries.Any(c => c.CountryName == country.CountryName);
            }
            return _context.Countries.Any(c => c.CountryName == country.CountryName && c.CountryId != country.CountryId);
        }

        public List<Country> GetAll()
        {
            return _context.Countries.AsNoTracking().ToList();
        }

        public Country? GetById(int id)
        {
            return _context.Countries.AsNoTracking().SingleOrDefault(c => c.CountryId == id);
        }

        public bool ItsRelated(int id)
        {
            return _context.States.Any(p => p.CountryId == id);
        }

        public void Update(Country country)
        {
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }

            _context.Countries.Update(country);

        }

    }
}
