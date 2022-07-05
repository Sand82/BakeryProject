using Bakery.Models.Author;
using Bakery.Service.Authors;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthorController(IAuthorService authorService, IWebHostEnvironment webHostEnvironment)
        {
            this.authorService = authorService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> About()
        {
            var authorModel = await authorService.GetAuthorInfo();

            if (authorModel == null)
            {
                return NotFound();
            }

            var modelEmployes = await authorService.GetModels();

            authorModel.Employees = modelEmployes;

            return View(authorModel);
        }

        [Authorize]
        public IActionResult Apply()
        {      
            return View(new ApplyFormModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyFormModel apply, IFormFile cv, IFormFile image)
        {
            var isValidFileFormat = authorService.FileValidator(
                cv.FileName, image.FileName, cv.Length, image.Length);            

            if (!isValidFileFormat)
            {
                ModelState.AddModelError("Autobiography", "Invalid file format or length.");
            }

            if (!ModelState.IsValid)  
            {               
                return View(apply);
            }            

            var employee =  authorService.CreateEmployee(apply);

            employee.FileExtension = CreateFile(cv, employee.FileId);
            employee.ImageExtension = CreateFile(image, employee.ImageId);

            await authorService.AddEmployee(employee);

            return RedirectToAction("About", "Author");
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
