using Bakery.Data;
using Bakery.Data.Models;

using BakeryServices.Models.Author;
using BakeryServices.Service.Employees;
using Microsoft.EntityFrameworkCore;

namespace BakeryServices.Service.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly BakeryDbContext data;
        private readonly IEmployeeService employeeService;

        public AuthorService(BakeryDbContext data, IEmployeeService employeeService)
        {
            this.data = data;
            this.employeeService = employeeService;
        }

        public Employee CreateEmployee(ApplyFormModel model)
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
                    FileId = model.FileId,
                    ImageId = model.ImageId,
                };

            }).GetAwaiter().GetResult();

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

        public async Task<List<EmployeeDetailsViewModel>> GetModels()
        {
            var models = await this.data
             .Employees
             .Where(e => e.IsApproved == true)
             .Select(e => new EmployeeDetailsViewModel
             {
                 FullName = e.FullName,
                 Description = e.Description,
                 Experience = e.Experience,
                 Image = employeeService.GetExstention(e.ImageId, e.ImageExtension)
             })
             .ToListAsync();

            return models;
        }

        public async Task<AuthorViewModel> GetAuthorInfo()
        {
            var author = await this.data.Authors
               .Select(a => new AuthorViewModel
               {
                   Id = a.Id,
                   FirstName = a.FirstName,
                   LastName = a.LastName,
                   Description = a.Description,
                   ImageUrl = a.ImageUrl,
               })
               .FirstOrDefaultAsync();

            return author;
        }

        public async Task<bool> IsAuthor(string userId)
        {
            var isValidAuthor = await this.data.Authors
                .AnyAsync(a => a.AuthorId == userId);

            return isValidAuthor;
        }

        public async Task AddEmployee(Employee employee)
        {
            await this.data.Employees.AddAsync(employee);

            await this.data.SaveChangesAsync();
        }
    }
}
