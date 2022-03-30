using Bakery.Areas.Job.Models;
using Bakery.Data.Models;

namespace Bakery.Service
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeViewModel> GetAllApplies();

        Employee GetById(int id);

        void SetEmployee(Employee employee, bool isApprove);
    }
}
