using Bakery.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bakery.Tests.GlobalMethods
{
    public static class TestService
    {
        public static string ConvertToJason(object obj)
        {
            var result = JsonConvert.SerializeObject(obj);

            return result;
        }        

        public static List<Order> CreateListOrders()
        {
            var items = CreateListItem();

            var orders = new List<Order>();

            for (int i = 1; i <= 5; i++)
            {
                var order = new Order()
                {
                    Id = i,
                    DateOfOrder = DateTime.Now,
                    DateOfDelivery = DateTime.Now,
                    UserId = $"some user{i}",
                    Items = items
                };

                orders.Add(order);
            }

            return orders;
        }

        public static List<Item> CreateListItem()
        {
            var items = new List<Item>();

            for (int i = 1; i <= 5; i++)
            {
                var item = new Item
                {
                    Id = i,
                    ProductName = $"Bread{i}",
                    ProductPrice = 3.20m + i,
                    ProductId = i,
                    Quantity = i,
                };

                items.Add(item);
            }

            return items;
        }
    }
}
