using Bakery.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bakery.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            var scolpedServices = app.ApplicationServices.CreateScope();

            var data = scolpedServices.ServiceProvider.GetService<BackeryDbContext>();

            data.Database.Migrate();            

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
    }
}
