using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class ShoppingCartsRepository : GenericRepository<ShoppingCart>, IShoppingCartsRepository
    {
        private readonly Gardens2024DbContext _db;

        public ShoppingCartsRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(ShoppingCart shoppingCart)
        {

            _db.ShoppingCarts.Update(shoppingCart);

        }
    }
}
