using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ICategoriesRepository:IGenericRepository<Category>
    {
        void Update(Category category);
        bool Exist(Category category);
        bool ItsRelated(int id);
    }
}
