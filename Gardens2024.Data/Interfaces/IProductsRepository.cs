using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IProductsRepository:IGenericRepository<Product>
    {
        void Update(Product product);
        bool Exist(Product product);
        bool ItsRelated(int id);
    }
}
