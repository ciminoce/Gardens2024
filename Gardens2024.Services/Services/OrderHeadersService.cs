using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Entities.Entities;
using Gardens2024.Services.Interfaces;
using System.Linq.Expressions;

namespace Gardens2024.Services.Services
{
    public class OrderHeadersService : IOrderHeadersService
    {
        private readonly IOrderHeadersRepository? _repository;
        private readonly IProductsRepository? _productsRepository;
        private readonly IShoppingCartsRepository? _shoppingCartsRepository;
        private readonly IUnitOfWork? _unitOfWork;

        public OrderHeadersService(IOrderHeadersRepository? repository,
            IProductsRepository productsRepository,
            IShoppingCartsRepository shoppingCartsRepository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository?? throw new ArgumentException("Dependencies not set");
            _productsRepository = productsRepository?? throw new ArgumentException("Dependencies not set");
            _shoppingCartsRepository = shoppingCartsRepository?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Delete(orderHeader);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.Rollback();
                throw;
            }
        }
        public OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }
        public void Save(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (orderHeader.OrderHeaderId==0)
                {
                    _repository?.Add(orderHeader);
                    //_unitOfWork.SaveChanges();
                    foreach (var item in orderHeader.OrderDetails)
                    {
                        var productInDetail = _productsRepository.Get(
                            filter: p => p.ProductId == item.ProductId);
                        productInDetail.Stock -= item.Quantity;
                        productInDetail.StockInCarts-= item.Quantity;
                        _productsRepository.Update(productInDetail);

                        var shoppingCart = _shoppingCartsRepository.Get(
                                filter: sc => sc.ProductId == item.ProductId
                                && sc.ApplicationUserId == orderHeader.ApplicationUserId);
                        _shoppingCartsRepository.Delete(shoppingCart);
                    }
                }
                else
                {
                    _repository?.Update(orderHeader);
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
