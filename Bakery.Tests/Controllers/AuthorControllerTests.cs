using Bakery.Controllers;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Service;
using Bakery.Tests.Mock;

using Xunit;

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

            var authorService = new AuthorService(data, null);

            var authorController = new AuthorController(authorService);

            var result = authorService.GetAuthorInfo();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<AuthorViewModel>(result);

            Assert.Equal("Sand", result.FirstName);
            Assert.Equal("Stef", result.LastName);
        }

        [Fact]
        public void AboutActionShouldReturnBadRequestViewWhitEmptyDatabase()
        {
            using var data = DatabaseMock.Instance;

            var authorService = new AuthorService(data, null);

            var authorController = new AuthorController(authorService);

            var result = authorService.GetAuthorInfo();

            Assert.Null(result);
        }       
    }
}
