using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;

namespace Gardens2024.Data.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly Gardens2024DbContext _db;

        public CategoriesRepository(Gardens2024DbContext db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public bool Exist(Category category)
        {

            if (category.CategoryId == 0)
            {
                return _db.Categories.Any(c => c.CategoryName == category.CategoryName);
            }
            return _db.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);
        }


        public bool ItsRelated(int id)
        {
            return _db.Products.Any(p => p.CategoryId == id);
        }

        public void Update(Category category)
        {

            _db.Categories.Update(category);

        }
    }
}
