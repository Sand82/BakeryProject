using Bakery.Data;
using Bakery.Data.Models;
using BakeryServices.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace BakeryServices.Service.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BakeryDbContext data;

        public EmployeeService(BakeryDbContext data)
        {
            this.data = data;
        }

        public async Task<ICollection<EmployeeViewModel>> GetAllApplies()
        {
            var employees = await this.data
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
            .ToListAsync();

            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await this.data
            .Employees
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();

            return employee;
        }

        public async Task<EmployeeInfoViewModel> GetModelById(int id)
        {
            var model = new EmployeeInfoViewModel();

            var employee = await this.data
            .Employees
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new NullReferenceException("Not Found");
            }

            model.Name = employee.FullName;
            model.Description = employee.Description;
            model.Age = employee.Age;
            model.Experience = employee.Experience;
            model.FilePath = GetExstention(employee.FileId, employee.FileExtension);
            model.Image = GetExstention(employee.ImageId, employee.ImageExtension);

            return model;
        }

        public string GetExstention(string id, string exstension)
        {
            var path = $"/files/" + id + '.' + exstension;

            return path;
        }

        public async Task SetEmployee(Employee employee, bool isApprove)
        {
            employee.IsApproved = isApprove;

            await this.data.SaveChangesAsync();
        }

        public string GetContentType(string path)
        {
            var tokens = path.Split('.').ToArray();

            var extention = tokens[tokens.Length - 1];

            string contentType = null;

            try
            {
                contentType = ContentTypeColection(extention);
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Not supported file extension.");
            }

            return contentType;
        }

        private string ContentTypeColection(string extention)
        {
            var extentions = new Dictionary<string, string>()
            {
                {"txt", "text/plain" },
                {"pdf", "application/pdf"},
                {"doc", "application/vnd.ms-word"},
                {"docx", "application/vnd.ms-word" },
                {"odt", "application/vnd.oasis.opendocument.text" },
            };

            string needetExtention = null;

            try
            {
                needetExtention = extentions[extention];
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("Not Found");
            }

            return needetExtention;
        }
    }
}
