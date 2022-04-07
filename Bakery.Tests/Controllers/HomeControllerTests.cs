using Bakery.Controllers;
using Bakery.Data.Models;
using Bakery.Models.Home;
using Bakery.Service;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void HomeControllerShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var products = Enumerable.Range(0, 10)
                .Select(p => new Product
                {
                    Name = "Boza",
                    Description = "BozaBozaBozaBozaBozaBozaBoza",
                    Price = 3.25m,
                    ImageUrl = "Nqma.gpeg",
                    Ingredients = Enumerable.Range(0, 2)
                    .Select(i => new Ingredient
                    {
                        Name = "Oshte Boza",                         
                    })
                    .ToList()
                }); ; 

            data.Products.AddRange(products);

            data.SaveChanges();

            var homeService = new HomeService(data);
            var homeController = new HomeController(homeService);

            var result = homeController.Index();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<CountViewModel>(model);
                        
            Assert.Equal(20, indexViewModel.IngredientCount);
            Assert.Equal(10, indexViewModel.ProductCount);
        }            
    }
}
