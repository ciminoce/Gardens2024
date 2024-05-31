using Gardens2024.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gardens2024.Data
{
    public class Gardens2024DbContext:DbContext
    {
        public Gardens2024DbContext(DbContextOptions<Gardens2024DbContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
