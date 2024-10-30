using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly Gardens2024DbContext _db;

        public OrderDetailsRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(OrderDetail orderDetail)
        {

            _db.OrderDetails.Update(orderDetail);

        }
    }
}
