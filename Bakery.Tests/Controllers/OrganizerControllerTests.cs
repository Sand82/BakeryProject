using Bakery.Areas.Task.Controllers;
using BakeryData;
using BakeryServices.Service.Organizers;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class OrganizerControllerTests
    {
        [Fact]
        public void RequestActionShoulReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var date = DateTime.UtcNow;            

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Request;

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);
        }

        private OrganizerController CreateClaimsPrincipal(BakeryDbContext data)
        {
            var organizerService = new OrganizerService(data);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new OrganizerController(organizerService);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }
    }
}
