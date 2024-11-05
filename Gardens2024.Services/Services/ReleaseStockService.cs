using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gardens2024.Services.Services
{
    public class ReleaseStockService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReleaseStockService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Resuelve el servicio scoped en cada ciclo, dentro del alcance
                    var cartsRepository = scope.ServiceProvider.GetRequiredService<IShoppingCartsRepository>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    unitOfWork.BeginTransaction();
                    try
                    {
                        DateTime cutoffTime = DateTime.Now.AddHours(-1);
                        var inactiveCarts = cartsRepository.GetAll(filter: c => c.LastUpdated <= cutoffTime);

                        foreach (var cart in inactiveCarts)
                        {
                            var productInCart = productRepository.Get(filter: p => p.ProductId == cart.ProductId);
                            productInCart.StockInCarts -= cart.Quantity;
                            productRepository.Update(productInCart);
                            cartsRepository.Delete(cart); // Elimina el carrito inactivo
                        }

                        unitOfWork.Commit(); // Confirma los cambios
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback(); // Revertir cambios si hay un error
                        throw;
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // Espera antes de la siguiente ejecución
            }
        }
    }

}
