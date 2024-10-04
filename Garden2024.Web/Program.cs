using Gardens2024.Data;
using Gardens2024.Ioc;
using Gardens2024.Utilities;
using Microsoft.AspNetCore.Identity;
namespace Garden2024.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<Gardens2024DbContext>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });

            builder.Services.AddRazorPages();//Para que trabaje con vistas de Razor
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            DI.ConfigurarServicios(builder.Services, builder.Configuration);
            // Configurar AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAndAdminUser(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();//Para verificar tipo de usuario

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Hero}/{id?}");

            app.Run();
        }
        private static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync(WC.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Admin));
            }
            if (!await roleManager.RoleExistsAsync(WC.Role_Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Customer));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123.");

                // Asignar el rol de Admin al usuario
                await userManager.AddToRoleAsync(adminUser, WC.Role_Admin);
            }

        }

    }
}
