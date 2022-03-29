using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;

namespace Bakery.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BackeryDbContext data;

        public EmployeeService(BackeryDbContext data)
        {
            this.data = data;
        }

        public IEnumerable<EmployeeViewModel> GetAllApplies()
        {
            var employees = this.data
                .Employees
                .Where(e => e.IsApproved == null )
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,
                    Description = x.Description,
                    Experience = x.Experience,
                    Autobiography = x.Autobiography,
                    IsApproved = true,
                })
                .ToList();

            return employees;                
        }

        public Employee GetById(int id)
        {
            var employee = this.data
                .Employees
                .Where(e => e.Id == id)
                .Select(x => new Employee
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,
                    Description = x.Description,
                    Experience = x.Experience,
                    Autobiography = x.Autobiography,
                    IsApproved = true,
                })
                .FirstOrDefault();

            return employee;
        }

        public void SetEmployee(Employee employee)
        {
            employee.IsApproved = true;

            this.data.SaveChanges();
        }
    }
}
