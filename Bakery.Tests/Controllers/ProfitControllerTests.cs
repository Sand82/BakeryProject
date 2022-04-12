using Bakery.Areas.Task.Controllers;
using Bakery.Areas.Task.Models;
using Bakery.Data;
using Bakery.Service;
using Bakery.Tests.Mock;

using System;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class ProfitControllerTests
    {
        [Fact]
        public void ShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;            

            var model = new CheckFormModel
            {
                ValueFrom = "10.04.2022",
                ValueTo = DateTime.Now.ToString("dd.MM.yyyy"),
            };

            var controller = CreateController(data);

            var result = controller.Check(model);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<String>(result);        
        }

        [Fact]
        public void ShouldReturnInCorectResultWhenMethodDateIsIncorectFormat()
        {
            using var data = DatabaseMock.Instance;

            var model = new CheckFormModel
            {
                ValueFrom = "10/04/2022",
                ValueTo = DateTime.Now.ToString("dd/MM/yyyy"),
            };

            var controller = CreateController(data);

            var result = controller.Check(model);            

            var viewResult = Assert.IsType<String>(result);

            Assert.Equal("Invalid data format or data period", viewResult);
        }

        [Fact]
        public void ShouldReturnInCorectResultWhenDateFormIsErlierThanDateTo()
        {
            using var data = DatabaseMock.Instance;

            var model = new CheckFormModel
            {
                ValueTo = "10/04/2022",
                ValueFrom = DateTime.Now.ToString("dd/MM/yyyy"),
            };

            var controller = CreateController(data);

            var result = controller.Check(model);

            var viewResult = Assert.IsType<String>(result);

            Assert.Equal("Invalid data format or data period", viewResult);
        }

        private ProfitController CreateController(BakeryDbContext data)
        {
            var orders = CreateListOrders();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var organizerService = new OrganizerService(data);

            var orderService = new OrderService(data);

            var controller = new ProfitController(orderService, organizerService);

            return controller;
        }
    }
}
