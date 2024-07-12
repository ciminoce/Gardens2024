using Gardens2024.Entities.Entities;
using System.Linq.Expressions;

namespace Gardens2024.Services.Interfaces
{
    public interface ICategoriesService
    {
        IEnumerable<Category>? GetAll(Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            string? propertiesNames = null);
        void Save(Category category);
        void Delete(Category category);
        Category? Get(Expression<Func<Category, bool>>? filter = null,
            string? propertiesNames = null, 
            bool tracked=true);
        bool Exist(Category category);
        bool ItsRelated(int id);

    }
}
