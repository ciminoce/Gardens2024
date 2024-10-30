using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class OrderHeadersRepository : GenericRepository<OrderHeader>, IOrderHeadersRepository
    {
        private readonly Gardens2024DbContext _db;

        public OrderHeadersRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(OrderHeader orderHeader)
        {

            _db.OrderHeaders.Update(orderHeader);

        }
    }
}
