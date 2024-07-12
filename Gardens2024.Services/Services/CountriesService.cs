using Gardens2024.Data.Interfaces;
using Gardens2024.Data;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class CountriesService:ICountriesService
    {
        private readonly ICountriesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CountriesService(ICountriesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(Country category)
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


        public bool Exist(Country category)
        {

            return _repository!.Exist(category);
        }

        public Country? Get(Expression<Func<Country, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<Country> GetAll(Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }


        public bool ItsRelated(int id)
        {

            return _repository!.ItsRelated(id);
        }

        public void Save(Country category)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (category.CountryId == 0)
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
