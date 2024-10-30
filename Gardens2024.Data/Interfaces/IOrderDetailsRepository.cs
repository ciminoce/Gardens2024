using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface IOrderDetailsRepository:IGenericRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
