using Bakery.Controllers;
using Bakery.Data;
using Bakery.Models.Items;
using Bakery.Service;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class ItemControllerTests
    {
        [Fact]
        public void DetailsShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;            

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var productId = 1;

            var result = controller.Details(productId);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<DetailsViewModel>(model);

            var expected = products.Where(x => x.Id == productId).FirstOrDefault();

            Assert.Equal(expected.Name, indexViewModel.Name);
            Assert.Equal(expected.Price.ToString("f2"), indexViewModel.Price);
        }

        private ItemController CreateClaimsPrincipal(BakeryDbContext data)
        {
            var voteService = new VoteService(data);

            var itemsService = new ItemsService(data, voteService);

            var orderService = new OrderService(data);

            var bakerySevice = new BakerySevice(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new ItemController(
                itemsService, voteService, orderService, bakerySevice);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }
    }
}
