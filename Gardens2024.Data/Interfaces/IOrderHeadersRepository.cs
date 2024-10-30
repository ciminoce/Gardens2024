using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IOrderHeadersRepository:IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
