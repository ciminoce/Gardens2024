using Gardens2024.Entities.Entities;

namespace Gardens2024.Services.Interfaces
{
    public interface ICategoriesService
    {
        List<Category>? GetAll();
        void Save(Category category);
        void Delete(int id);
        Category? GetById(int id);
        bool Exist(Category category);
        bool ItsRelated(int id);

    }
}
