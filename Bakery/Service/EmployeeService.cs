using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;

namespace Bakery.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BackeryDbContext data;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeService(BackeryDbContext data, IWebHostEnvironment webHostEnvironment)
        {
            this.data = data;
            this.webHostEnvironment = webHostEnvironment;
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
                    Age = x.Age,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,                    
                    Experience = x.Experience,                    
                    ApplayDate = x.ApplayDate.ToString("dd.MM.yyyy"),                    
                    IsApproved = true,
                })
                .ToList();

            }).GetAwaiter().GetResult();

            return employees;
        }

        public Employee GetEmployeeById(int id)
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

        public EmployeeInfoViewModel GetModelById(int id)
        {
            var model = new EmployeeInfoViewModel();            

            var employee = new Employee();

            Task.Run(() =>
            {
                employee = this.data
               .Employees
               .Where(e => e.Id == id)
               .FirstOrDefault();

                model.Name = employee.FullName;
                model.Description = employee.Description;
                model.Age = employee.Age;
                model.Experience = employee.Experience;
                model.File = GetExstention(employee.FileId);

            }).GetAwaiter().GetResult();           
                   

            return model;
        } 
        
        private string GetExstention(string id)
        {
            var path = string.Empty;



            return path;
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
