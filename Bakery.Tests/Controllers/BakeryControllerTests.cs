using Bakery.Controllers;
using Bakery.Models.Bakeries;
using Bakery.Service;
using Bakery.Tests.Mock;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class BakeryControllerTests
    {
        [Fact]
        public void AllShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();
            
            data.Products.AddRange(product);            

            data.SaveChanges();

            var bakeryService = new BakerySevice(data);

            var authorService = new AuthorService(data, null);

            var bakeryController = new BakeryController(bakeryService, authorService, data);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.NameIdentifier, "Sand82"),
                new Claim(ClaimTypes.Name, "sand@abv.bg")                                       
            }, "TestAuthentication"));

            var controller = new BakeryController(bakeryService, authorService, data);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var result = controller.All(new AllProductQueryModel());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<AllProductQueryModel>(model);

            var expect = product.Where(x => x.IsDelete == false).ToList();
           
            Assert.Equal(expect.Count, indexViewModel.TotalProduct);
        }        
    }
}
