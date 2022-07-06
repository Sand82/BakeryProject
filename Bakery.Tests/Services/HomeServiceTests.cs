using Bakery.Data.Models;
using BakeryServices.Service.Home;
using Bakery.Tests.Mock;

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bakery.Tests.Services
{
    public class HomeServiceTests
    {
        [Fact]
        public async Task HomeServiceShouldReturnCorrectProductCount()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            await data.Products.AddRangeAsync(product);

            await data.SaveChangesAsync();

            var homeService = new HomeService(data);

            var resultProduct = homeService.GetIndex().GetAwaiter().GetResult().ProductCount;           
            
            Assert.StrictEqual(5, resultProduct);    
            
        }

        [Fact]
        public async Task HomeServiceShouldReturnCorrectIngredientsCount()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var homeService = new HomeService(data);

            var ingrediantResult = homeService.GetIndex().GetAwaiter().GetResult().IngredientCount;

            Assert.StrictEqual(3, ingrediantResult);          

        }

        [Fact]
        public async Task HomeServiceShouldReturnCountOfZeroWhenEmptyDatabase()
        {
            using var data = DatabaseMock.Instance;            

            var homeService = new HomeService(data);

            var ingrediantResult = homeService.GetIndex().GetAwaiter().GetResult().IngredientCount;
            var resultProduct = homeService.GetIndex().GetAwaiter().GetResult().ProductCount;

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
