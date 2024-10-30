using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface IOrderHeadersService
    {
        IEnumerable<OrderHeader>? GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null);
        void Save(OrderHeader OrderHeader);
        void Delete(OrderHeader OrderHeader);
        OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null,
            string? propertiesNames = null, 
            bool tracked=true);
    }
}
