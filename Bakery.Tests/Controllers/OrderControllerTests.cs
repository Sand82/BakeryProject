using Bakery.Controllers;
using Bakery.Data;
using Bakery.Models.Customer;
using Bakery.Service;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public void BuyActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            orders[0].UserId = "vqra@abv.bg";

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Buy();

            Assert.NotNull(result); 

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            Assert.NotNull(model);

            var indexViewModel = Assert.IsType<CustomerFormModel>(model);

            Assert.Equal(1, indexViewModel.OrderId);           
        }

        [Fact]
        public void BuyActionShouldReturnBadRequestResultWhenUserIdIsIncorect()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var customerSevice = new CustomerService(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, ""),
                new Claim(ClaimTypes.Name, "")

           }, "TestAuthentication"));

            var controller = new OrderController(orderService, customerSevice);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var result = controller.Buy();            

            var viewResult = Assert.IsType<BadRequestResult>(result);           
        }

        private OrderController CreateClaimsPrincipal(BakeryDbContext data)
        {
            
            var orderService = new OrderService(data);

            var customerSevice = new CustomerService(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new OrderController(orderService, customerSevice);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }
    }
}
