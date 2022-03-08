using Bakery.Data;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public class AboutService : IAboutService
    {
        private readonly BackeryDbContext data;

        public AboutService(BackeryDbContext data)
        {
            this.data = data;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            var authorInfo = this.data.Authors.FirstOrDefault();

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

        public bool IsAuthor(string userId)
        {
            return this.data.Authors.Any(x => x.UserId == userId);
        }
    }
}
