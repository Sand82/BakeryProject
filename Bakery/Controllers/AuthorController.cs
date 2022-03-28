using Bakery.Models.Author;
using Bakery.Service;
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
            var author = authorService.GetAuthorInfo();

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [Authorize]
        public IActionResult Applay()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult Apply(ApplyFormModel apply, IFormFile cv)
        {
            return RedirectToAction("About", "Author");
        }
    }
}
