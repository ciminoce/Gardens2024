using Gardens2024.Data.Interfaces;
using Gardens2024.Data;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;

namespace Gardens2024.Services.Services
{
    public class CountriesService:ICountriesService
    {
        private readonly ICountriesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CountriesService(ICountriesRepository? repository,
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

        public bool Exist(Country country)
        {
            if (_repository is null)
            {
                throw new ApplicationException("Dependencies not loaded!!");
            }

            return _repository.Exist(country);
        }

        public List<Country>? GetAll()
        {
            return _repository?.GetAll();
        }

        public Country? GetById(int id)
        {
            return _repository?.GetById(id);
        }

        public bool ItsRelated(int id)
        {
            if (_repository is null)
            {
                throw new ApplicationException("Dependencies not loaded!!");
            }

            return _repository.ItsRelated(id);
        }

        public void Save(Country country)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (country.CountryId == 0)
                {
                    _repository?.Add(country);
                }
                else
                {
                    _repository?.Update(country);
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
