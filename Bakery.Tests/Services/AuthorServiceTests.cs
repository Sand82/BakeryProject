using Bakery.Data.Models;
using Bakery.Service.Authors;
using Bakery.Tests.Mock;

using Xunit;

namespace Bakery.Tests.Services
{
    public class AuthorServiceTests
    {
        [Fact]
        public void GetAuthorInfoReturnCorrectValue()
        {
            using var data = DatabaseMock.Instance;

            var author = AutorFactory();    
            
            data.Authors.Add(author);

            data.SaveChanges();

            var authorService = new AuthorService(data, null, null);

            var result = authorService.GetAuthorInfo();                            

            Assert.NotNull(result);             
        }

        [Fact]
        public void GetAuthorInfoReturnNullIfDatabaseIsEmpty()
        {
            using var data = DatabaseMock.Instance;            

            var authorService = new AuthorService(data, null, null);

            var result = authorService.GetAuthorInfo();

            Assert.Null(result);
        }

        [Fact]
        public void IsAuthorReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            string authorId = "my id";

            var author = AutorFactory();

            author.AuthorId = authorId;

            data.Authors.Add(author);

            data.SaveChanges();

            var authorService = new AuthorService(data, null, null);

            var result = authorService.IsAuthor(authorId);            

            Assert.True(result);
        }

        [Fact]
        public void IsAuthorReturnNullWhenIdIsIncorrect()
        {
            using var data = DatabaseMock.Instance;

            string authorId = "my id";

            var author = AutorFactory();

            author.AuthorId = "2";

            data.Authors.Add(author);

            data.SaveChanges();

            var authorService = new AuthorService(data, null, null);

            var result = authorService.IsAuthor(authorId);

            Assert.False(result);
        }

        [Fact]
        public void FileValidatorReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;          
          
            var authorService = new AuthorService(data, null, null);

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

            var authorService = new AuthorService(data, null, null);

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


