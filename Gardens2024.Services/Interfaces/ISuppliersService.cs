using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface ISuppliersService
    {
        IEnumerable<Supplier>? GetAll(Expression<Func<Supplier, bool>>? filter = null,
            Func<IQueryable<Supplier>, IOrderedQueryable<Supplier>>? orderBy = null,
            string? propertiesNames = null);
        void Save(Supplier supplier);
        void Delete(Supplier supplier);
        Supplier? Get(Expression<Func<Supplier, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        bool Exist(Supplier supplier);
        bool ItsRelated(int id);

    }
}
