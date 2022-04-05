using Xunit;
using MyTested.AspNetCore.Mvc;
using Bakery.Controllers;
using Bakery.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Bakery.Models.Home;

namespace Bakery.Test.Controller
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexControllerShouldReturnCorectDataModell()

         => MyController<HomeController>
            .Instance(controller => controller
                .WithRouteData(GetProducts()))
                 .Calling(p => p.Index())
                    .ShouldReturn()
                    .ActionResult()
                    .ShouldPassForThe<CountViewModel>(p => p.AllProductViewModel.Count() == 11);                 
             
          
        private static IEnumerable<Product> GetProducts()
        {
            var products = Enumerable.Range(0, 10)
                .Select(i => new Product());

            return products;
        }
    }
}
