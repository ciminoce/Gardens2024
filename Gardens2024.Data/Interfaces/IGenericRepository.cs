using System.Linq.Expressions;

namespace Gardens2024.Data.Interfaces
{
    public interface IGenericRepository<T>where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? filter=null,
            Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null,
            string? propertiesNames=null);
        T? Get(Expression<Func<T, bool>>? filter = null,
            string? propertiesNames = null, bool tracked=true);
        void Add(T entity);
        void Delete(T entity);

    }
}
