using Bakery.Controllers;
using BakeryData;
using BakeryServices.Models.Customer;
using BakeryServices.Service.Customers;
using BakeryServices.Service.Orders;
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

        //[Fact]
        //public void BuyPostActionShouldReturnCorectResult()
        //{
        //    using var data = DatabaseMock.Instance;

        //    var orders = CreateListOrders();

        //    orders[0].UserId = "vqra@abv.bg";

        //    data.Orders.AddRange(orders);

        //    data.SaveChanges();

        //    var formModel = new CustomerFormModel
        //    {
        //        FirstName = "Sand",
        //        LastName = "Stef",
        //        Address = "test test test test",
        //        PhoneNumber = "+359888777666",
        //        Email = "sand@abv.bg",
        //        OrderId = 1,
        //        UserId = "vqra@abv.bg",
        //        Order = new CreateOrderModel
        //        {
        //            Id = 1,
        //            DateOfDelivery = "18.04.2022",
        //            DateOfOrder = "02.04.2022",
        //            IsFinished = true,
        //            ItemsCount = 3,
        //            TotallPrice = "80.00$",
        //            items = new List<ItemFormViewModel>()
        //            {
        //                new ItemFormViewModel
        //                {
        //                    Name = "Bread",
        //                    Price = "3,20",
        //                    Quantity = 3,
        //                    UserId = "vqra@abv.bg",
        //                },
        //                new ItemFormViewModel
        //                {
        //                    Name = "Bread1",
        //                    Price = "3,30",
        //                    Quantity = 2,
        //                    UserId = "vqra@abv.bg",
        //                },
        //                new ItemFormViewModel
        //                {
        //                    Name = "Bread2",
        //                    Price = "3,40",
        //                    Quantity = 1,
        //                    UserId = "vqra@abv.bg",
        //                },

        //            }
        //            .ToList()
        //        }
        //    };

        //    var controller = CreateClaimsPrincipal(data);

        //    controller.TempData.Add(SuccessOrder, "Order added seccessfully.");

        //    var result = controller.Buy(formModel);

        //    Assert.NotNull(result);
        //}

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
