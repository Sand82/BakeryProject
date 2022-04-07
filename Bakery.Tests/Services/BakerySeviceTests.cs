﻿using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Service;
using Bakery.Tests.Mock;
using System.Collections.Generic;
using Xunit;

namespace Bakery.Tests.Services
{
    public class BakerySeviceTests
    {

        [Fact]
        public void GetAllProductsReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();                   
                        
            model.CurrentPage = 1;
            model.Categories = categoryList;            
            model.SearchTerm = string.Empty;
            model.Category = string.Empty;

           var result = bakerySevice.GetAllProducts(model);

            Assert.NotNull(result);
            Assert.StrictEqual(10, result.TotalProduct);
          }

        private List<Product> ProductsCollection()
        {
            ICollection<Ingredient> ingredients = new HashSet<Ingredient>(){new Ingredient { Id = 1, Name = "Ingredient1"  },
                new Ingredient { Id = 2, Name = "Ingredient2" },
                new Ingredient { Id = 3, Name = "Ingredient3" } };

            var products = new List<Product>();

            for (int i = 1; i <= 10; i++)
            {
                var product = new Product { Id = i, Name = $"Bread{i}", IsDelete = false, Description = "Bread Bread Bread Bread Bread.", Price = 3.20m, ImageUrl = $"nqma{i}.png", Ingredients = ingredients };

                products.Add(product);

            }

            for (int i = 11; i <= 15; i++)
            {
                var product = new Product { Id = i, Name = $"Bread{i}", IsDelete = true, Description = "Bread Bread Bread Bread Bread.", Price = 3.20m, ImageUrl = $"nqma{i}.png", Ingredients = ingredients };

                products.Add(product);

            }

            return products;
        }
    }
}