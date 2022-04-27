using Bakery.Infrastructure;
using Bakery.Models.Contacts;
using Bakery.Service.Contacts;
using Bakery.Service.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
   
    public class ContactController : Controller
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [Authorize]
        public IActionResult Location()
        {          

            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            var model = contactService.FindModel(userId);

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

            contactService.CreateMail(contactModel);

            return RedirectToAction("All", "Bakery");
        }
    }
}
