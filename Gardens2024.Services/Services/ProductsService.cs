using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ProductsService(IProductsRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(Product product)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(product);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }


        public bool Exist(Product product)
        {

            return _repository!.Exist(product);
        }

        public Product? Get(Expression<Func<Product, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null,
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }


        public bool ItsRelated(int id)
        {

            return _repository!.ItsRelated(id);
        }

        public void Save(Product product)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (product.ProductId==0)
                {
                    _repository?.Add(product);
                }
                else
                {
                    _repository?.Update(product);
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
