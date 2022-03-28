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
            throw new NotImplementedException();
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
    }
}
