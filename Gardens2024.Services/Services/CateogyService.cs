using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;

namespace Gardens2024.Services.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CategoriesService(ICategoriesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                _repository?.Delete(id);
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.Rollback();
                throw;
            }
        }

        public bool Exist(Category category)
        {
            return _repository?.Exist(category)??true;
        }

        public List<Category>? GetAll()
        {
            return _repository?.GetAll();
        }

        public Category? GetById(int id)
        {
            return _repository?.GetById(id);
        }

        public bool ItsRelated(int id)
        {
            return _repository?.ItsRelated(id)??true;
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
