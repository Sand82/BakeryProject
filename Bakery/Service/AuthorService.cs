using Bakery.Data;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly BackeryDbContext data;

        public AuthorService(BackeryDbContext data)
        {
            this.data = data;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            var authorInfo = this.data.Authors.FirstOrDefault();

            if (authorInfo == null)
            {
                return null;
            }

            var author = new AuthorViewModel
            {
                Id = authorInfo.Id,
                FirstName = authorInfo.FirstName,
                LastName = authorInfo.LastName,
                Description = authorInfo.Description,
                ImageUrl = authorInfo.ImageUrl,
            };

            return author;
        }
    }
}
