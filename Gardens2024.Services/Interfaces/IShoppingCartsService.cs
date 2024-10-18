using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface IShoppingCartsService
    {
        IEnumerable<ShoppingCart>? GetAll(Expression<Func<ShoppingCart, bool>>? filter = null,
            Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null,
            string? propertiesNames = null);
        void Save(ShoppingCart shoppingCart);
        void Delete(ShoppingCart shoppingCart);
        ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null,
            string? propertiesNames = null, 
            bool tracked=true);

    }
}
