using Bakery.Controllers;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Items;
using Bakery.Service;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class ItemControllerTests
    {
        [Fact]
        public void DetailsShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var productId = 1;

            var result = controller.Details(productId);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<DetailsViewModel>(model);

            var expected = products.Where(x => x.Id == productId).FirstOrDefault();

            Assert.Equal(expected.Name, indexViewModel.Name);
            Assert.Equal(expected.Price.ToString("f2"), indexViewModel.Price);
        }

        [Fact]
        public void DetailsShouldReturnNotFoundWithUncorectProductId()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var productId = 20;

            var result = controller.Details(productId);

            var viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.True(viewResult.StatusCode == 404);
        }

        [Fact]
        public void DetailsPostShouldWorkCorectly()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Details(1, 3);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("All", viewResult.ActionName);
            Assert.Equal("Bakery", viewResult.ControllerName);
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(1, 0)]
        [InlineData(1, 2001)]
        public void DetailsPostShouldRedirectWithUncorectProductCount(int productId, int quantity)
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Details(productId, quantity);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectResult>(result);
        }

        [Theory]
        [InlineData(16, 4)]
        [InlineData(-1, 3)]             
        public void DetailsPostShouldReturnBadRequestWithUncorectProductId(int productId, int quantity)
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Details(productId, quantity);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<BadRequestResult>(result);

            Assert.True(viewResult.StatusCode == 400);
        }

        [Fact]
        public void DetailsPostShouldReturnBadRequestWhenUseIdIsNull()
        {
            using var data = DatabaseMock.Instance;

            var voteService = new VoteService(data);

            var itemsService = new ItemsService(data, voteService);

            var orderService = new OrderService(data);

            var bakerySevice = new BakerySevice(data);

            var controller = new ItemController(
                itemsService, voteService, orderService, bakerySevice);

            var result = controller.Details(1, 3);            

            var viewResult = Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public void VoteActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Vote(1, 5);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Details", viewResult.ActionName);
            Assert.Equal("Item", viewResult.ControllerName);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(-1, 5)]
        [InlineData(1, 10)]        
        [InlineData(1, 6)]        
        public void VoteActionShouldReturnBadRequestWhenIdIsZero(int id, byte vote)
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Vote(id, vote);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<BadRequestResult>(result);           

        }

        [Fact]
        public void VoteActionShouldReturnBadRequestWhenProductIdNotExistDatabase()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Vote(16, 5);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<BadRequestResult>(result);          

        }

        [Fact]
        public void EditAllActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItems();

            var order = new Order
            {
                Id = 1,
                DateOfOrder = DateTime.UtcNow,
                DateOfDelivery = DateTime.UtcNow,
                IsFinished = true,
                Items = items,
                UserId = "test",
            };

            order.DateOfOrder.AddDays(3);

            data.Orders.AddRange(order);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.EditAll(1);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<List<EditItemsFormModel>>(model);

            Assert.Equal(5, indexViewModel.Count);
        }

        [Fact]
        public void EditAllActionShouldReturnNotFoundResultWhenItemNotExistInDatabase()
        {
            using var data = DatabaseMock.Instance;

            var controller = CreateClaimsPrincipal(data);

            var result = controller.EditAll(1);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<NotFoundObjectResult>(result);           
        }

        [Fact]
        public void DeleteAllActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItems();

            var order = new Order
            {
                Id = 1,
                DateOfOrder = DateTime.UtcNow,
                DateOfDelivery = DateTime.UtcNow,
                IsFinished = true,
                Items = items,
                UserId = "test",
            };

            order.DateOfOrder.AddDays(3);

            data.Orders.AddRange(order);

            data.SaveChanges();

            int orderId = 1;

            var controller = CreateClaimsPrincipal(data);

            var result = controller.DeleteAll(orderId);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result); 
            
            var currItems = data.Orders
                .Where(o => o.Id == orderId)
                .Sum(i => i.Items.Count());

            Assert.Equal("Buy", viewResult.ActionName);
            Assert.Equal("Order", viewResult.ControllerName);
            Assert.True(currItems == 0);
        }

        [Fact]
        public void DeleteAllActionShouldReturnNotFoundResultWhenItemNotExistInDatabase()
        {
            using var data = DatabaseMock.Instance;

            var controller = CreateClaimsPrincipal(data);

            var result = controller.DeleteAll(1);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<BadRequestResult>(result);
        }

        private ItemController CreateClaimsPrincipal(BakeryDbContext data)
        {
            var voteService = new VoteService(data);

            var itemsService = new ItemsService(data, voteService);

            var orderService = new OrderService(data);

            var bakerySevice = new BakerySevice(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new ItemController(
                itemsService, voteService, orderService, bakerySevice);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }
    }
}
