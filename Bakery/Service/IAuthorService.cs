using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public interface IAuthorService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);

        bool FileValidator(IFormFile cv, IFormFile image);

        Employee CreateEmployee(ApplyFormModel model, IFormFile cv, IFormFile image);

        void AddEmployee(Employee employee);
    }
}
