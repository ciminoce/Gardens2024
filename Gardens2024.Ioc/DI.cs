using Gardens2024.Data;
using Gardens2024.Data.Interfaces;
using Gardens2024.Data.Repositories;
using Gardens2024.Services.Interfaces;
using Gardens2024.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gardens2024.Ioc
{
    public static class DI
    {
        public static void ConfigurarServicios(IServiceCollection servicios, IConfiguration configuration)
        {


            servicios.AddScoped<ICategoriesRepository,
                CategoriesRepository>();
            servicios.AddScoped<ICategoriesService, CategoriesService>();

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
