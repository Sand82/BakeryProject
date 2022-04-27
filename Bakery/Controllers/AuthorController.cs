using Bakery.Models.Author;
using Bakery.Service.Authors;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [Authorize]
        public IActionResult About()
        {
            var authorModel = authorService.GetAuthorInfo();

            if (authorModel == null)
            {
                return NotFound();
            }

            var modelEmployes = authorService.GetModels();

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
        public IActionResult Apply(ApplyFormModel apply, IFormFile cv, IFormFile image)
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

            var employee = authorService.CreateEmployee(apply, cv, image);

            authorService.AddEmployee(employee);

            return RedirectToAction("About", "Author");
        }
    }
}
