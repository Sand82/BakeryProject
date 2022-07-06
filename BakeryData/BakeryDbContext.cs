using BakeryData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BakeryData
{
    public class BakeryDbContext : IdentityDbContext
    {
               
        public BakeryDbContext(DbContextOptions<BakeryDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }       

        public DbSet<Product> Products { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }  
        
        public DbSet<Customer> Customers { get; set; }     
        
        public DbSet<Vote> Votes { get; set; }  
        
        public DbSet<Employee> Employees { get; set; }  

        public DbSet<MailInfo> MailInfos { get; set; }  

                       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
                        
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }    
}