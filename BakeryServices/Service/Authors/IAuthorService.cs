using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service.Authors
{
    public interface IAuthorService
    {
        AuthorViewModel GetAuthorInfo();

        bool IsAuthor(string userId);

        bool FileValidator(
            string fileExtension, string imageExtension, long fileLingth, long imigeLength);

        Employee CreateEmployee(ApplyFormModel model);

        List<EmployeeDetailsViewModel> GetModels();

        void AddEmployee(Employee employee);
    }
}
