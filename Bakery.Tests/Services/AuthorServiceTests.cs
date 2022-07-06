using BakeryData.Models;
using Bakery.Tests.Mock;
using BakeryServices.Service.Authors;
using System.Threading.Tasks;
using Xunit;

namespace Bakery.Tests.Services
{
    public class AuthorServiceTests
    {
        [Fact]
        public async Task GetAuthorInfoReturnCorrectValue()
        {
            using var data = DatabaseMock.Instance;

            var author = AutorFactory();    
            
            await data.Authors.AddAsync(author);

            await data.SaveChangesAsync();

            var authorService = new AuthorService(data, null);

            var result = await authorService.GetAuthorInfo();                            

            Assert.NotNull(result);             
        }

        [Fact]
        public async Task GetAuthorInfoReturnNullIfDatabaseIsEmpty()
        {
            using var data = DatabaseMock.Instance;            

            var authorService = new AuthorService(data, null);

            var result = await authorService.GetAuthorInfo();

            Assert.Null(result);
        }

        [Fact]
        public async Task IsAuthorReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            string authorId = "my id";

            var author = AutorFactory();

            author.AuthorId = authorId;

            await data.Authors.AddAsync(author);

            await data.SaveChangesAsync();

            var authorService = new AuthorService(data, null);

            var result = await authorService.IsAuthor(authorId);            

            Assert.True(result);
        }

        [Fact]
        public async Task IsAuthorReturnNullWhenIdIsIncorrect()
        {
            using var data = DatabaseMock.Instance;

            string authorId = "my id";

            var author = AutorFactory();

            author.AuthorId = "2";

            await data.Authors.AddAsync(author);

            await data.SaveChangesAsync();

            var authorService = new AuthorService(data, null);

            var result = await authorService.IsAuthor(authorId);

            Assert.False(result);
        }

        [Fact]
        public void FileValidatorReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;          
          
            var authorService = new AuthorService(data, null);

            var result = authorService.FileValidator("fail.doc", "snimka.png", 123456, 123456);

            Assert.True(result);
        }

        [Theory]
        [InlineData("fail.gog", "snimka.png", 123456, 123456)]
        [InlineData("fail.doc", "snimka.pngggr", 123456, 123456)]
        [InlineData("fail.doc", "snimka.png", 123456, 12345655657)]
        [InlineData("fail.doc", "snimka.png", 12345698347, 123456)]

        public void FileValidatorReturnIncorectResult(
            string fileExtension, string imageExtention, long  fileLength, long imageLength)
        {
            using var data = DatabaseMock.Instance;

            var authorService = new AuthorService(data, null);

            var result = authorService.FileValidator(fileExtension, imageExtention, fileLength, imageLength);

            Assert.False(result);
        }

        private Author AutorFactory()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "Sand",
                LastName = "Stef",
                Description = "test test test",
                ImageUrl = "nqma.png",
            };

            return author;
        }
    }
}


