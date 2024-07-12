using Gardens2024.Data.Interfaces;
using Gardens2024.Data;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class StatesService:IStatesService
    {
        private readonly IStatesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public StatesService(IStatesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(State state)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(state);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }


        public bool Exist(State state)
        {

            return _repository!.Exist(state);
        }

        public State? Get(Expression<Func<State, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<State> GetAll(Expression<Func<State, bool>>? filter = null,
            Func<IQueryable<State>, IOrderedQueryable<State>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }


        public bool ItsRelated(int id)
        {

            return _repository!.ItsRelated(id);
        }

        public void Save(State state)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (state.StateId == 0)
                {
                    _repository?.Add(state);
                }
                else
                {
                    _repository?.Update(state);
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
