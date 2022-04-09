using Bakery.Data.Models;
using Bakery.Service;
using Bakery.Tests.Mock;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bakery.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreatOrderShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var userId = "Some id";

            var result = orderService.CreatOrder(userId);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreatItemShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var userId = "Some id";

            var result = orderService.CreateItem(1, "Bread", 3.20m, 3, userId);

            Assert.NotNull(result);
        }

        [Fact]
        public void FindOrderByUserIdShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var userId = "User2";

            var result = orderService.FindOrderByUserId(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public void FindOrderByUserIdShouldReturnZeroWhitIncorectMethodParameter()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var userId = "OtherUser2";

            var result = orderService.FindOrderByUserId(userId);

            Assert.Null(result);
            
        }

        private List<Order> CreateListOrders()
        {
            var orders = new List<Order>();

            var items = CreateListItem();

            for (int i = 1; i <= 5; i++)
            {
                var order = new Order
                {
                    Id = i,
                    DateOfOrder = DateTime.UtcNow,
                    IsFinished = false,
                    Items = items,
                    UserId = $"User{i}",
                };

                orders.Add(order);
            }

            return orders;
        }

        private List<Item> CreateListItem()
        {
            var items = new List<Item>();

            for (int i = 1; i < 6; i++)
            {
                var item = new Item
                {
                    Id = i,
                    ProductName = $"Bread{i}",
                    ProductId = i,
                    ProductPrice = 3.20m + i,
                    Quantity = i,
                };

                items.Add(item);
            }

            return items;
        }
    }
}
