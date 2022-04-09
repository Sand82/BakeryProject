using Bakery.Data.Models;
using Bakery.Service;
using Bakery.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void CreateOrderModelShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var result = orderService.CreateOrderModel(orders[1]);

            Assert.NotNull(result);
            Assert.Equal(5, result.items.Count);
            Assert.Equal("70.00", result.TotallPrice);
        }

        [Fact]
        public void CreateOrderUserIdShouldReturn0totalPriceWithEmptyCollectioninOrder()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            orders[0].Items = new List<Item>();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var result = orderService.CreateOrderModel(orders[0]);

            Assert.Equal("0.00", result.TotallPrice);
            Assert.Equal(0, result.ItemsCount);
        }

        [Fact]
        public void FinishOrderShouldReturnCorectresult()
        {
            using var data = DatabaseMock.Instance;

            var orders = CreateListOrders();

            data.Orders.AddRange(orders);

            data.SaveChanges();

            var orderService = new OrderService(data);

            orderService.FinishOrder(orders[0], DateTime.UtcNow);

            var result = data.Orders.FirstOrDefault(x => x.UserId == "User1");

            Assert.True(result.IsFinished);

        }

        [Fact]
        public void GetCustomerShouldReturnCorectresult()
        {
            using var data = DatabaseMock.Instance;

            var customers = CreateListCustomers();

            data.Customers.AddRange(customers);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var result = orderService.GetCustomer("Sand82");

            Assert.Equal("Sand5", result.FirstName);         
            Assert.Equal("Stef5", result.LastName);         
        }

        [Fact]
        public void GetCustomerShouldReturnZeroWhenUserIdIsUncorect()
        {
            using var data = DatabaseMock.Instance;

            var customers = CreateListCustomers();

            data.Customers.AddRange(customers);

            data.SaveChanges();

            var orderService = new OrderService(data);

            var result = orderService.GetCustomer("Sand1982");

            Assert.Null(result);           
        }

        [Fact]
        public void TryParceDateShouldReturnTrue()
        {
            using var data = DatabaseMock.Instance;           

            var orderService = new OrderService(data);

            var result = orderService.TryParceDate("03.04.2022");

            Assert.True(result.Item1);                        
        }

        [Fact]
        public void TryParceDateShouldReturnFalseWhitIncorectInput()
        {
            using var data = DatabaseMock.Instance;

            var orderService = new OrderService(data);

            var result = orderService.TryParceDate("sand");

            Assert.False(result.Item1);
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

        private List<Customer> CreateListCustomers()
        {
            var customers = new List<Customer>();

            for (int i = 1; i < 6; i++)
            {
                var customer = new Customer
                {
                    Id = i,
                    FirstName = $"Sand{i}",
                    LastName = $"Stef{i}",
                    Adress = "Some adress",
                    PhoneNumber = "Some number",
                    Email = "some@abv.bg",                    
                    UserId = "Sand82"
                };

                customers.Add(customer);
            }

            return customers;
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
                    ProductPrice = 1.00m + i,
                    Quantity = i,
                };

                items.Add(item);
            }

            return items;
        }
    }
}
