using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface ICountriesService
    {
        IEnumerable<Country>? GetAll(Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            string? propertiesNames = null);
        void Save(Country country);
        void Delete(Country country);
        Country? Get(Expression<Func<Country, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked= true  );
        bool Exist(Country country);
        bool ItsRelated(int id);

    }
}
