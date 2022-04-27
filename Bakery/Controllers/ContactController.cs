using Bakery.Models.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class ContactController : Controller
    {
        [Authorize]
        public IActionResult Location()
        {
            var model = new ContactFormModel();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Location(ContactFormModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            return View();
        }
    }
}
