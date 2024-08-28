using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public SuppliersService(ISuppliersRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(Supplier supplier)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(supplier);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }


        public bool Exist(Supplier supplier)
        {

            return _repository!.Exist(supplier);
        }

        public Supplier? Get(Expression<Func<Supplier, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<Supplier> GetAll(Expression<Func<Supplier, bool>>? filter = null,
            Func<IQueryable<Supplier>, IOrderedQueryable<Supplier>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }


        public bool ItsRelated(int id)
        {

            return _repository!.ItsRelated(id);
        }

        public void Save(Supplier supplier)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (supplier.SupplierId == 0)
                {
                    _repository?.Add(supplier);
                }
                else
                {
                    _repository?.Update(supplier);
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
