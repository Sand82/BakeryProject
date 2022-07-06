using Bakery.Controllers;
using BakeryServices.Models.Items;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class VoteControllerTests
    {
        [Fact]
        public void PostShouldReturnCorectResult()
        {
            var modelData = new DetailsModel
            {
                productId = 1,
                Quantity = 2
            };

            var controller = new VoteController();

            var result = controller.Post(modelData);

            Assert.NotNull(result);

            Assert.IsType<RedirectToActionResult>(result);                     
        }

        [Fact]
        public void PostShouldRedirectViewWhenInputIsIncorect()
        {
            var modelData = new DetailsModel
            {
                productId = 2,
                Quantity = 2
            };

            var controller = new VoteController();

            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");

            var result = controller.Post(modelData);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.True(viewResult.ViewData.ModelState.Count > 0);           
        }
    }
}
