using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;
using Bakery.Service.Employees;

namespace Bakery.Service.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly BakeryDbContext data;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmployeeService employeeService;

        public AuthorService(BakeryDbContext data, IWebHostEnvironment webHostEnvironment, IEmployeeService employeeService)
        {
            this.data = data;
            this.webHostEnvironment = webHostEnvironment;
            this.employeeService = employeeService;
        }

        public Employee CreateEmployee(ApplyFormModel model, IFormFile cv, IFormFile image)
        {
            Employee employee = null;

            Task.Run(() =>
            {
                employee = new Employee
                {
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Age = model.Age,
                    Email = model.Email,
                    Description = model.Description,
                    Experience = model.Experience,
                };

            }).GetAwaiter().GetResult();

            employee.FileExtension = CreateFile(cv, employee.FileId);
            employee.ImageExtension = CreateFile(image, employee.ImageId);

            return employee;
        }

        public bool FileValidator(
            string fileExtension, string imageExtension, long fileLingth, long imigeLength)
        {
            var isValid = true;

            var fileExstention = Path.GetExtension(fileExtension).ToLower().Trim('.');

            var imageExstention = Path.GetExtension(imageExtension).ToLower().Trim('.');

            Task.Run(() =>
            {

                var commonFileFormats = new List<string>() { "doc", "docx", "odt", "txt", "pdf" };

                var commonImageFormats = new List<string>() { "png", "img", "jpeg", "gif", "jpg" };

                if (!commonFileFormats.Contains(fileExstention) || !commonImageFormats.Contains(imageExstention) ||
                fileLingth > 2 * 1024 * 1024 || imigeLength > 6 * 1024 * 1024)
                {
                    isValid = false;
                }

            }).GetAwaiter().GetResult();

            return isValid;
        }

        public List<EmployeeDetailsViewModel> GetModels()
        {
            List<EmployeeDetailsViewModel> models = null;

            Task.Run(() => 
            {
                models = this.data
                .Employees
                .Where(e => e.IsApproved == true)
                .Select(e => new EmployeeDetailsViewModel
                {
                    FullName = e.FullName,
                    Description = e.Description,
                    Experience = e.Experience,
                    Image = employeeService.GetExstention(e.ImageId, e.ImageExtension)
                })
                .ToList();

            }).GetAwaiter().GetResult();

            return models;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            AuthorViewModel? author = null;

            Task.Run(() =>
            {
                author = this.data.Authors
               .Select(a => new AuthorViewModel
               {
                   Id = a.Id,
                   FirstName = a.FirstName,
                   LastName = a.LastName,
                   Description = a.Description,
                   ImageUrl = a.ImageUrl,
               })
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return author;
        }

        public bool IsAuthor(string userId)
        {
            var isValidAuthor = true;

            Task.Run(() =>
            {
                isValidAuthor = this.data
                .Authors
                .Any(a => a.AuthorId == userId);

            }).GetAwaiter().GetResult();

            return isValidAuthor;
        }

        public void AddEmployee(Employee employee)
        {
            Task.Run(() =>
            {
                this.data.Employees.Add(employee);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }

        private string CreateFile(IFormFile file, string id)
        {
            Directory.CreateDirectory($"{webHostEnvironment.WebRootPath}/files/");

            var extension = Path.GetExtension(file.FileName).Trim('.');

            var path = $"{webHostEnvironment.WebRootPath}/files/{id}.{extension}";

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            };

            return extension;
        }        
    }
}
