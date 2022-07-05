using Bakery.Areas.Job.Models;
using Bakery.Data.Models;

namespace Bakery.Service.Employees
{
    public interface IEmployeeService
    {
        Task<ICollection<EmployeeViewModel>> GetAllApplies();

        Task<Employee> GetEmployeeById(int id);

        Task<EmployeeInfoViewModel> GetModelById(int id);

        Task SetEmployee(Employee employee, bool isApprove);

        string GetContentType(string path);

        string GetExstention(string id, string exstension);
    }
}
