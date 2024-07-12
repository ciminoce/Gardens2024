using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CategoriesService(ICategoriesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(Category category)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(category);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }


        public bool Exist(Category category)
        {

            return _repository!.Exist(category);
        }

        public Category? Get(Expression<Func<Category, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<Category> GetAll(Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }


        public bool ItsRelated(int id)
        {

            return _repository!.ItsRelated(id);
        }

        public void Save(Category category)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (category.CategoryId==0)
                {
                    _repository?.Add(category);
                }
                else
                {
                    _repository?.Update(category);
                }
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

    }
}
