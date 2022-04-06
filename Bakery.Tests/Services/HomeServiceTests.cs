using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Home;
using Bakery.Service;
using Bakery.Tests.Mock;
using System.Collections.Generic;
using Xunit;

namespace Bakery.Tests.Services
{
    public class HomeServiceTests
    {
        [Fact]
        public void HomeServiceShouldReturnCorrectProductCount()
        {
            var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var homeService = new HomeService(data);

            var resultProduct = homeService.GetIndex().ProductCount;           
            
            Assert.StrictEqual(5, resultProduct);    
            
        }

        [Fact]
        public void HomeServiceShouldReturnCorrectIngredientsCount()
        {
            var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var homeService = new HomeService(data);

            var ingrediantResult = homeService.GetIndex().IngredientCount;

            Assert.StrictEqual(3, ingrediantResult);          

        }

        [Fact]
        public void HomeServiceShouldReturnCountOfZeroWhenEmptyDatabase()
        {         
            var data = DatabaseMock.Instance;            

            var homeService = new HomeService(data);

            var ingrediantResult = homeService.GetIndex().IngredientCount;
            var resultProduct = homeService.GetIndex().ProductCount;

            Assert.StrictEqual(0, ingrediantResult);
            Assert.StrictEqual(0, resultProduct);
        }

        private List<Product> ProductsCollection()
        {
            ICollection<Ingredient> ingredients = new HashSet<Ingredient>(){new Ingredient { Id = 1, Name = "Ingredient1"  },
                new Ingredient { Id = 2, Name = "Ingredient2" },
                new Ingredient { Id = 3, Name = "Ingredient3" } };

            var products = new List<Product>();

            for (int i = 1; i <= 5; i++)
            {
                var product = new Product { Id = i, Name = $"Bread{i}", IsDelete = false, Description = "Bread Bread Bread Bread Bread.", Price= 3.20m, ImageUrl = $"nqma{i}.png", Ingredients = ingredients  };

                products.Add(product);

            }

            return products;
        }
    }
}
