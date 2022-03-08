using Bakery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService)
        {
            this.aboutService = aboutService;
        }

        public IActionResult Owner()
        {
            var author = aboutService.GetAuthorInfo();

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
    }
}
