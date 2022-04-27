using Bakery.Areas.Job.Models;
using Bakery.Data.Models;

namespace Bakery.Service.Employees
{
    public interface IEmployeeService
    {
        ICollection<EmployeeViewModel> GetAllApplies();

        Employee GetEmployeeById(int id);

        EmployeeInfoViewModel GetModelById(int id);

        void SetEmployee(Employee employee, bool isApprove);

        string GetContentType(string path);

        string GetExstention(string id, string exstension);
    }
}
