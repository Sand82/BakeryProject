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

        public static List<Product> ProductsCollection()
        {
            var ingredients = GetIngredients();

            var category = GetCategory();

            var products = new List<Product>();

            for (int i = 1; i <= 10; i++)
            {
                var product = new Product
                {
                    Id = i,
                    Name = $"Bread{i}",
                    IsDelete = false,
                    Description = "Bread Bread Bread Bread Bread.",
                    Price = 3.20m,
                    ImageUrl = $"nqma{i}.png",
                    Ingredients = ingredients,
                    Category = category,
                    CategoryId = category.Id
                };

                products.Add(product);

            }

            for (int i = 11; i <= 15; i++)
            {
                var product = new Product { Id = i, Name = $"Bread{i}", IsDelete = true, Description = "Bread Bread Bread Bread Bread.", Price = 3.20m, ImageUrl = $"nqma{i}.png", Ingredients = ingredients };

                products.Add(product);
            }

            return products;
        }

        public static ICollection<Ingredient> GetIngredients()
        {
            var ingredients = new HashSet<Ingredient>(){new Ingredient { Id = 1, Name = "Ingredient1"  },
                new Ingredient { Id = 2, Name = "Ingredient2" },
                new Ingredient { Id = 3, Name = "Ingredient3" } };

            return ingredients;
        }

        public static Category GetCategory()
        {
            return new Category { Id = 1, Name = "Bread" };
        }

    }
}
