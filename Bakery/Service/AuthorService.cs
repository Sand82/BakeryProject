using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;
using Microsoft.AspNetCore.Identity;

namespace Bakery.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly BackeryDbContext data;        

        public AuthorService(BackeryDbContext data)
        {
            this.data = data;
        }        

        public Employee CreateEmployee(ApplyFormModel model, IFormFile cv)
        {
            Employee employee = new Employee(); 

            Task.Run(() => 
            {
                var cvByte = CreateByteFile(cv);

                employee = new Employee
                {
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Email = model.Email,
                    Description = model.Description,
                    Experience = model.Experience,
                    Autobiography = cvByte,
                };

            }).GetAwaiter().GetResult();          
                       
            return employee;
        }

        public bool FileValidator(IFormFile cv)
        {
            var isValid = true;

            Task.Run(() => 
            {
                var fileData = cv.FileName.Split('.').ToList();

                var fileFormat = fileData[fileData.Count - 1];

                var commonFormats = new List<string>() { "doc", "docx", "odt", "txt", "PDF" };

                if (!commonFormats.Contains(fileFormat) || cv.Length > 2 * 1024 * 1024)
                {
                    isValid = false;
                }

            }).GetAwaiter().GetResult();

            

            return isValid;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            var authorInfo = new Author();

            Task.Run(() =>
            {
                 authorInfo = this.data.Authors.FirstOrDefault();                          
                             
            }).GetAwaiter().GetResult();

            var author = new AuthorViewModel
            {
                Id = authorInfo.Id,
                FirstName = authorInfo.FirstName,
                LastName = authorInfo.LastName,
                Description = authorInfo.Description,
                ImageUrl = authorInfo.ImageUrl,
            };

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

        private byte[] CreateByteFile(IFormFile file)
        {
            var cvInMemory = new MemoryStream();

            file.CopyTo(cvInMemory);

            var cvBytes = cvInMemory.ToArray();

            return cvBytes;
        }
    }
}
