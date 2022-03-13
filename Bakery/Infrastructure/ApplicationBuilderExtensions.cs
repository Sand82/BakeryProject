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

            SeedAdministrator(serviceProvider);
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
                .Run(async() =>
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
    }
}
