using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class ShoppingCartsService : IShoppingCartsService
    {
        private readonly IShoppingCartsRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ShoppingCartsService(IShoppingCartsRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(shoppingCart);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }



        public ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>>? filter = null,
            Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }



        public void Save(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (shoppingCart.ShoppingCartId==0)
                {
                    _repository?.Add(shoppingCart);
                }
                else
                {
                    _repository?.Update(shoppingCart);
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
