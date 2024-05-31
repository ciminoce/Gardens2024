using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Interfaces
{
    public interface ICategoriesRepository
    {
        List<Category>? GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        Category? GetById(int id);
        bool Exist(Category category);
        bool ItsRelated(int id);
    }
}
