using Bakery.Data;
using Bakery.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static Bakery.WebConstants;

namespace Bakery.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {       

        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            var scolpedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scolpedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<BackeryDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            SeedAdministrator(serviceProvider);

            SeedAuthor(data);

            //SeedDayOfTheWeek(data)

            return app;
        }

        private static void SeedDayOfTheWeek(BackeryDbContext data)
        {
            //if (data.DayOfTheWeek.Any())
            //{
            //    return;
            //}

            //data.DayOfTheWeek.AddRange(new[]
            //{
            //    new DayOfTheWeek{ Name = "Monday"},
            //});
        }

        private static void SeedAdministrator(IServiceProvider service)
        {
            var userMager = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    var author = new IdentityUser
                    {
                        Email = "vqra@abv.bg",
                        UserName = "vqra@abv.bg",
                    };

                    await userMager.CreateAsync(author, "123456");

                    await userMager.AddToRoleAsync(author, role.Name);                   
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedCategories(BackeryDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Breds" },
                new Category { Name = "Cookies" },
                new Category { Name = "Sweets " },
                new Category { Name = "Specialty" },                
                new Category { Name = "Cakes" },                
            });

           data.SaveChanges();
        }

        private static void SeedAuthor(BackeryDbContext data) 
        {
            if (data.Authors.Any())
            {
                return;
            }

            data.Authors.Add(new Author
            {
                FirstName = "Vqra",
                LastName = "Hristova",
                Description = "Great baker. Always baking with the hart.",
                ImageUrl = "https://thatbreadlady.com/wp-content/uploads/2020/10/that-bread-lady-about-me-683x1024.jpg"

            });

            data.SaveChanges();
        }
    }
}
