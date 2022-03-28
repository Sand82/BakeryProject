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
            var cvByte = CreateByteFile(cv);

            var employee = new Employee
            {
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                Description = model.Description,
                Experience = model.Experience,
                Autobiography = cvByte,
            };

            return employee;
        }

        public bool FileValidator(IFormFile cv)
        {
            var isValid = true;

            var fileData = cv.FileName.Split('.').ToList();

            var fileFormat = fileData[fileData.Count - 1];

            var commonFormats = new List<string>() { "doc", "docx", "odt", "txt", "PDF" };

            if (!commonFormats.Contains(fileFormat) || cv.Length > 2 * 1024 * 1024)
            {
                isValid = false;
            }

            return isValid;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            var authorInfo = this.data.Authors.FirstOrDefault();

            if (authorInfo == null)
            {
                return null;
            }           

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
            return this.data
                .Authors
                .Any(a => a.AuthorId == userId);
        }

        public void AddEmployee(Employee employee)
        {
            this.data.Employees.Add(employee);

            this.data.SaveChanges();
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
