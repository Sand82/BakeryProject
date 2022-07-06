using BakeryData.Models;
using BakeryServices.Models.Employees;

namespace BakeryServices.Service.Employees
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
