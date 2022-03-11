using Bakery.Service;
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

        public IActionResult About()
        {
            var author = authorService.GetAuthorInfo();

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
    }
}
