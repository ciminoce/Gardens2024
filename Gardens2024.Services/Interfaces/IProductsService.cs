using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product>? GetAll(Expression<Func<Product, bool>>? filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
            string? propertiesNames = null);
        void Save(Product product);
        void Delete(Product product);
        Product? Get(Expression<Func<Product, bool>>? filter = null,
            string? propertiesNames = null, 
            bool tracked=true);
        bool Exist(Product product);
        bool ItsRelated(int id);

    }
}
