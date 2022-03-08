using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public interface IAboutService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);
    }
}
