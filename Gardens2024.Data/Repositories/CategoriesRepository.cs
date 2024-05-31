using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gardens2024.Data.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly Gardens2024DbContext _context;

        public CategoriesRepository(Gardens2024DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _context.Categories.Add(category);
            
        }

        public void Delete(int id)
        {
            var categoryInDb = GetById(id);
            if (categoryInDb != null)
            {
                _context.Categories.Remove(categoryInDb);
                
            }
            else
            {
                throw new Exception("Record deleted by other user!!!");
            }
        }

        public bool Exist(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (category.CategoryId == 0)
            {
                return _context.Categories.Any(c => c.CategoryName == category.CategoryName);
            }
            return _context.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);
        }

        public List<Category> GetAll()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories.AsNoTracking().SingleOrDefault(c => c.CategoryId == id);
        }

        public bool ItsRelated(int id)
        {
            return _context.Products.Any(p => p.CategoryId == id);
        }

        public void Update(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _context.Categories.Update(category);
            
        }
    }
}
