using BakeryServices.Service.Organizers;
using Bakery.Tests.Mock;
using System;
using System.Threading.Tasks;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Services
{
    public class OrganizerServiceTests
    {
        [Fact]
        public async Task GetCustomProfitShouldReturnCorectResult()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();

            order[0].IsFinished = true;

            await data.Orders.AddRangeAsync(order);            

            await data.SaveChangesAsync();            

            var organizerService = new OrganizerService(data);

            var dateTwo = DateTime.Now;

            var result = await organizerService.GetCustomProfit(dateOne, dateTwo);

            var expected = "103.00$";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetCustomProfitShouldReturn0IfNoProfitForThePeriod()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();           

            await data.Orders.AddRangeAsync(order);

            await data.SaveChangesAsync();

            var organizerService = new OrganizerService(data);

            var dateTwo = DateTime.Now;

            var result = await organizerService.GetCustomProfit(dateOne, dateTwo);

            var expected = "0.00$";

            Assert.NotNull(result);
            Assert.Equal(expected, result);

        }

        [Fact]
        public async Task GetItemsShouldReturnCorectResult()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();

            order[0].IsFinished = true;
            order[0].DateOfDelivery = dateOne.AddDays(1);

            order[1].IsFinished = true;
            order[1].DateOfDelivery = dateOne.AddDays(2);

            order[2].IsFinished = true;
            order[2].DateOfDelivery = dateOne.AddDays(3);

            order[3].IsFinished = true;
            order[3].DateOfDelivery = dateOne.AddDays(4);

            order[4].IsFinished = true;
            order[4].DateOfDelivery = dateOne.AddDays(5);

            await data.Orders.AddRangeAsync(order);

            await data.SaveChangesAsync();

            var organizerService = new OrganizerService(data);            

            var result = await organizerService.GetItems(dateOne);                      

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetItemsShouldReturnIncorectResult()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();
            
            await data.Orders.AddRangeAsync(order);

            await data.SaveChangesAsync();

            var organizerService = new OrganizerService(data);

            var result = await organizerService.GetItems(dateOne);
            
            Assert.Equal(5 , result.Count);
        }
    }
}
