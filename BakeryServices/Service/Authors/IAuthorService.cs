using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service.Authors
{
    public interface IAuthorService
    {
        Task<AuthorViewModel> GetAuthorInfo();

        Task<bool> IsAuthor(string userId);

        bool FileValidator(
            string fileExtension, string imageExtension, long fileLingth, long imigeLength);

        Employee CreateEmployee(ApplyFormModel model);

        Task<List<EmployeeDetailsViewModel>> GetModels();

        Task AddEmployee(Employee employee);
    }
}
