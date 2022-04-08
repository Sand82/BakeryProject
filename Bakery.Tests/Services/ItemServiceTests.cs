﻿using Bakery.Data.Models;
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
        //GetAllItems

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
        private List<Item> CreateListItem()
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


        private Order CreateOrder()
        {
            var items = CreateListItem();

            var item = new Order()
            {
                Id = 1,
                DateOfOrder = DateTime.Now,
                UserId = "some user",
                
            };

            return item;
        }
    }
}
