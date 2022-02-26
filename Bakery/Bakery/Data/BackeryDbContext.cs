using Bakery.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data
{
    public class BackeryDbContext : IdentityDbContext
    {

        public BackeryDbContext()
        {
        }

        public BackeryDbContext(DbContextOptions<BackeryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }


        public DbSet<ProductsIngredients> ProductsIngredients { get; set; }


        public DbSet<Product> Products { get; set; }


        public DbSet<Order> Orders { get; set; }

        public DbSet<OrdersProducts> OrdersProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server = localhost; User Id = sa; Password = @Stefanov820605; Database = BackeryProject");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductsIngredients>().HasKey(x => new { x.BakedDishId, x.ProductId });

            modelBuilder.Entity<OrdersProducts>().HasKey(x => new { x.OrderId, x.BakedDishId });
        }
    }    
}