using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public interface IAuthorService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);

        bool FileValidator(IFormFile cv);

        Employee CreateEmployee(ApplyFormModel model, IFormFile cv);

        void AddEmployee(Employee employee);
    }
}
