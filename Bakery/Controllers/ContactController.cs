using Bakery.Infrastructure;
using Bakery.Models.Contacts;
using Bakery.Service.Contacts;

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
        public async Task<IActionResult> Location()
        {          

            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            var model = await contactService.FindModel(userId);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Location(ContactFormModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }

            var fullName = CreateFullName(contactModel.FirstName, contactModel.LastName);

            await contactService.AddMailToSendGrid(contactModel.Email, fullName, contactModel.Subject, contactModel.Massage);

            await contactService.CreateMail(contactModel);

            return RedirectToAction("All", "Bakery");
        }

        private string CreateFullName(string firstName, string secondName)
        {
            return firstName + secondName;
        }
    }
}
