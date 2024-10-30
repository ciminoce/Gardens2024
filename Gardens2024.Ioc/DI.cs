using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Data.Repositories;
using Gardens2024.Services.Interfaces;
using Gardens2024.Services.Services;
using Gardens2024.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gardens2024.Ioc
{
    public static class DI
    {
        public static void ConfigurarServicios(IServiceCollection servicios, IConfiguration configuration)
        {

            servicios.AddScoped<ICountriesRepository, CountriesRepository>();
            servicios.AddScoped<ICategoriesRepository, CategoriesRepository>();

            servicios.AddScoped<IStatesRepository, StatesRepository>();
            servicios.AddScoped<ICitiesRepository, CitiesRepository>();
            servicios.AddScoped<ISuppliersRepository, SuppliersRepository>();
            servicios.AddScoped<IProductsRepository, ProductsRepository>();

            servicios.AddScoped<IApplicationUsersRepository, ApplicationUsersRepository>();
            servicios.AddScoped<IShoppingCartsRepository, ShoppingCartsRepository>();

            servicios.AddScoped<IOrderHeadersRepository, OrderHeadersRepository>();
            servicios.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();

            servicios.AddScoped<ICountriesService, CountriesService>();
            servicios.AddScoped<ICategoriesService, CategoriesService>();

            servicios.AddScoped<IStatesService, StatesService>();
            servicios.AddScoped<ICitiesService, CitiesService>();

            servicios.AddScoped<ISuppliersService, SuppliersService>();
            servicios.AddScoped<IProductsService, ProductsService>();

            servicios.AddScoped<IEmailSender, EmailSender>();
            servicios.AddScoped<IApplicationUsersService, ApplicationUsersService>();

            servicios.AddScoped<IShoppingCartsService, ShoppingCartsService>();

            servicios.AddScoped<IOrderHeadersService, OrderHeadersService>();
            servicios.AddScoped<IUnitOfWork, UnitOfWork>();
            servicios.AddDbContext<Gardens2024DbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("MyConnection")
                    );
            });


        }
    }
}
