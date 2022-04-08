using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;

namespace Bakery.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BakeryDbContext data;        

        public EmployeeService(BakeryDbContext data)
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

            }).GetAwaiter().GetResult();                

            return model;
        } 
        
        public string GetExstention(string id, string exstension)
        {
            var path = $"/files/" + id + '.' + exstension;

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

        public string GetContentType(string path)
        {
            var tokens = path.Split('.').ToArray();

            var extention = tokens[tokens.Length - 1];

            var contentType = ContentTypeColection(extention);

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

            var needetExtention = extentions[extention];

            return needetExtention;
        }
    }
}
