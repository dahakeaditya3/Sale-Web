using Microsoft.EntityFrameworkCore;

namespace Mobify.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        { 
        
        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Sellers> Sellers { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }

    }
}
