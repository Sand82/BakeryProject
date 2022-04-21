using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public interface IAuthorService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);

        bool FileValidator(
            string fileExtension, string imageExtension, long fileLingth, long imigeLength);

        Employee CreateEmployee(ApplyFormModel model, IFormFile cv, IFormFile image);

        List<EmployeeDetailsViewModel> GetModels();

        void AddEmployee(Employee employee);
    }
}
