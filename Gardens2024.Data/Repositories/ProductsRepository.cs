using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        private readonly Gardens2024DbContext _db;

        public ProductsRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public bool Exist(Product product)
        {

            if (product.ProductId == 0)
            {
                return _db.Products
                    .Any(c => c.ProductName == product.ProductName
                        && c.CategoryId==product.CategoryId);
            }
            return _db.Products.Any(c => c.ProductName == product.ProductName
            && c.CategoryId == product.CategoryId
            && c.ProductId != product.ProductId);
        }


        public bool ItsRelated(int id)
        {
            return false;
        }


        public void Update(Product product)
        {

            _db.Products.Update(product);

        }
    }
}
