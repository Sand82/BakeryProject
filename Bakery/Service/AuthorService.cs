﻿using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Author;
using Bakery.Models.Bakeries;

namespace Bakery.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly BakeryDbContext data;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthorService(BakeryDbContext data, IWebHostEnvironment webHostEnvironment)
        {
            this.data = data;
            this.webHostEnvironment = webHostEnvironment;
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

        public bool FileValidator(IFormFile cv, IFormFile image)
        {
            var isValid = true;

            var fileExstention = Path.GetExtension(cv.FileName).ToLower().Trim('.');

            var imageExstention = Path.GetExtension(image.FileName).ToLower().Trim('.');

            Task.Run(() =>
            {               

                var commonFileFormats = new List<string>() { "doc", "docx", "odt", "txt", "pdf"};

                var commonImageFormats = new List<string>() {"png", "img", "jpeg", "gif", "jpg" };

                if (!commonFileFormats.Contains(fileExstention) || !commonImageFormats.Contains(imageExstention) ||
                cv.Length > 2 * 1024 * 1024 || image.Length > 6 * 1024 * 1024)
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
