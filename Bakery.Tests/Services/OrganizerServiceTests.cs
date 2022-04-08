using Bakery.Service;
using Bakery.Tests.Mock;
using System;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Services
{
    public class OrganizerServiceTests
    {
        [Fact]
        public void GetCustomProfitShouldReturnCorectResult()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();

            order[0].IsFinished = true;

            data.Orders.AddRange(order);            

            data.SaveChanges();            

            var organizerService = new OrganizerService(data);

            var dateTwo = DateTime.Now;

            var result = organizerService.GetCustomProfit(dateOne, dateTwo);

            var expected = "103.00$";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCustomProfitShouldReturn0IfNoProfitForThePeriod()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();           

            data.Orders.AddRange(order);

            data.SaveChanges();

            var organizerService = new OrganizerService(data);

            var dateTwo = DateTime.Now;

            var result = organizerService.GetCustomProfit(dateOne, dateTwo);

            var expected = "0.00$";

            Assert.NotNull(result);
            Assert.Equal(expected, result);

        }//GetItems

        [Fact]
        public void GetItemsShouldReturnCorectResult()
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

            data.Orders.AddRange(order);

            data.SaveChanges();

            var organizerService = new OrganizerService(data);            

            var result = organizerService.GetItems(dateOne);                      

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetItemsShouldReturnIncorectResult()
        {
            var dateOne = DateTime.Now;

            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();
            
            data.Orders.AddRange(order);

            data.SaveChanges();

            var organizerService = new OrganizerService(data);

            var result = organizerService.GetItems(dateOne);
            
            Assert.Equal(5 , result.Count);
        }
    }
}
