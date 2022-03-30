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
            var employees = new List<EmployeeViewModel>();

            Task.Run(() =>
            {
                employees = this.data
                .Employees
                .Where(e => e.IsApproved == null)
                .OrderByDescending(e => e.ApplayDate)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,
                    Description = x.Description,
                    Experience = x.Experience,
                    Autobiography = x.Autobiography,
                    ApplayDate = x.ApplayDate.ToString("dd.MM.yyyy"),
                    Image = x.Image,
                    IsApproved = true,
                })
                .ToList();

            }).GetAwaiter().GetResult();

            return employees;
        }

        public Employee GetById(int id)
        {
            var employee = new Employee();

            Task.Run(() =>
            {
                employee = this.data
               .Employees
               .Where(e => e.Id == id)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return employee;
        }

        public void SetEmployee(Employee employee, bool isApprove)
        {
            Task.Run(() =>
            {
                employee.IsApproved = isApprove;

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }
    }
}
