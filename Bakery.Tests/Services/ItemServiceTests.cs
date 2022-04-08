using Bakery.Data.Models;
using Bakery.Service;
using Bakery.Tests.Mock;
using System;
using System.Collections.Generic;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Services
{
    public class ItemServiceTests
    {       
        [Fact]
        public void FindItemShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var item = CreateItem();

            data.Items.Add(item);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result =  itemsService.FindItem("Bread", 3, 3.20m);

            var obj1Str = ConvertToJason(result);
            var obj2Str = ConvertToJason(item);

            Assert.Equal(obj1Str, obj2Str);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(null, 1, 3.20 )]
        [InlineData("Bread", 2, 3.20)]
        [InlineData("Bread",  1, 3.80)]
       
        public void FindItemShouldReturnNullResultWithIncorectMethodParameters(string name, int quantity, decimal currPrice)
        {
            using var data = DatabaseMock.Instance;

            var item = CreateItem();

            data.Items.Add(item);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindItem(name, quantity, currPrice);
            
            Assert.Null(result);
        }        

        [Fact]
        public void FindOrderByIdReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateOrder();

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindOrderById(1);            
                       
            Assert.NotNull(result);
        }

        [Fact]
        public void FindOrderByIdReturnZeroResultWithIncorectMethodParameter()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateOrder();

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindOrderById(2);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllItemsReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateOrder();            

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.GetAllItems(1);

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetAllItemsReturnZeroWithEmptyOrder()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateOrder();

            order.Items = new List<Item>();

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.GetAllItems(1);

            Assert.Empty(result);           
        }

        [Fact]
        public void GetAllItemsReturnExeprionWithIncoredMethodparameter()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateOrder();            

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);            

            var ex = Assert.Throws<System.NullReferenceException>(() => itemsService.GetAllItems(2));

            Assert.Equal("Not found", ex.Message);

        }

        [Fact]
        public void GetDetailsReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;            

            var product = new Product
            {
                Id = 1,
                Name = "Bread",
                Price = 3.20m,
                Description = "BreadBreadBreadBread",
                ImageUrl = "Bread.png",
                Category = new Category { Id = 1, Name = "Bread" },
            };

            data.Products.Add(product);

            data.Votes.Add(new Vote() { Id = 1, ProductId = 1, UsreId = "some key" });

            data.SaveChanges();

            var itemsService = new ItemsService(data, new VoteService(data));

            var result = itemsService.GetDetails(1, "some key");

            Assert.NotNull(result);            
        }

        [Fact]
        public void FindOrderByUserIdReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);           

            var result = itemsService.FindOrderByUserId("some user3");

            Assert.NotNull(result);
        }

        [Fact]
        public void FindOrderByUserIdReturnZeroResultWhenMethodParametarIsWrong()
        {
            using var data = DatabaseMock.Instance;

            var order = CreateListOrders();

            data.Orders.AddRange(order);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindOrderByUserId("some user6");

            Assert.Null(result);
        }        

        [Fact]
        public void FindItemByIdReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItem();

            data.Items.AddRange(items);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindItemById(3);

            Assert.NotNull(result);
        }

        [Fact]
        public void FindItemByIdReturnNullWhenMethodParameturNotExsist()
        {
            using var data = DatabaseMock.Instance;

            var items = CreateListItem();

            data.Items.AddRange(items);

            data.SaveChanges();

            var itemsService = new ItemsService(data, null);

            var result = itemsService.FindItemById(10);

            Assert.Null(result);
        }

        private Item CreateItem()
        {
            var item = new Item()
            {
                Id = 1,
                ProductName = "Bread",
                ProductPrice = 3.20m,
                ProductId = 1,
                Quantity = 3,
            };

            return item;
        }
      
        private Order CreateOrder()
        {
            var items = CreateListItem();

            var order = new Order()
            {
                Id = 1,
                DateOfOrder = DateTime.Now,
                UserId = "some user",  
                Items = items
            };

            return order;
        }        
    }
}
