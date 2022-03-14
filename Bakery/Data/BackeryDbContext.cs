using Bakery.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data
{
    public class BackeryDbContext : IdentityDbContext
    {
               
        public BackeryDbContext(DbContextOptions<BackeryDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<ProductsIngredients> ProductsIngredients { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrdersProducts> OrdersProducts { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductsIngredients>().HasKey(pi => new { pi.ProductId, pi.IngredientId });

            modelBuilder.Entity<OrdersProducts>().HasKey(x => new { x.OrderId, x.ProductId });
                        
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }    
}