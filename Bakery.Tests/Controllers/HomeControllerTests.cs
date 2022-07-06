using Bakery.Controllers;
using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Home;
using BakeryServices.Service.Home;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void HomeActionShouldReturnCorectResult()
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

            var controller = CreateController(data);

            var result = controller.Index();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<CountViewModel>(model);
                        
            Assert.Equal(20, indexViewModel.IngredientCount);
            Assert.Equal(10, indexViewModel.ProductCount);
        }

        //[Fact]
        //public void ErrorActionShouldReturnCorectResult()
        //{
        //    using var data = DatabaseMock.Instance;           
            
        //    var controller = CreateController(data);            

        //    var result = controller.Error().ExecuteResultAsync;

        //    Assert.NotNull(result);

        //    var viewResult = Assert.IsType<ViewResult>(result);

        //    var model = viewResult.Model;

        //    var indexViewModel = Assert.IsType<ErrorViewModel>(model);
        //}

        public HomeController CreateController(BakeryDbContext data)
        {
            var homeService = new HomeService(data);

            var controller = new HomeController(homeService);

            return controller;
        }
    }
}
