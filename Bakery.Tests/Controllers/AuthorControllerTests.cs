using Bakery.Controllers;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;
using Bakery.Service;
using Bakery.Tests.Mock;
using Microsoft.AspNetCore.Mvc;
using Xunit;

using static Bakery.Tests.Mock.FormFileMock;

namespace Bakery.Tests.Controllers
{
    public class AuthorControllerTests
    {
        [Fact]
        public void AboutActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var author = new Author
            {
                Id = 1,
                FirstName = "Sand",
                LastName = "Stef",
                ImageUrl = "SomeImage.png",
                Description = "test descriptiona",

            };

            data.Authors.Add(author);

            data.SaveChanges();     

            var controller = CreateController(data);

            var result = controller.About();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<AuthorViewModel>(model);

            Assert.Equal("Sand", indexViewModel.FirstName);
            Assert.Equal("Stef", indexViewModel.LastName);
        }

        [Fact]
        public void ApplayActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var controller = CreateController(data);

            var result = controller.Apply();            

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);  
            
            var model = viewResult.Model;

            Assert.IsType<ApplyFormModel>(model);
        }

        [Fact]
        public void AboutActionShouldReturnBadRequestViewWhitEmptyDatabase()
        {
            using var data = DatabaseMock.Instance;            
          
            var controller = CreateController(data);

            var result = controller.About();

            Assert.IsType<NotFoundResult>(result);
        }    
        
        private AuthorController CreateController(BakeryDbContext data)
        {
            var authorService = new AuthorService(data, null);

            var controller = new AuthorController(authorService);

            return controller;
        }
    }
}
