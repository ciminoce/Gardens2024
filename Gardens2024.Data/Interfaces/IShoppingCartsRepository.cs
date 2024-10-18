using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IShoppingCartsRepository:IGenericRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart);
    }
}
