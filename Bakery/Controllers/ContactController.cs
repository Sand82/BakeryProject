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

            //   var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            //   Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "YOUR_API_KEY");

            //   var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("test@example.com", "Example User");
            //var subject = "Sending with Twilio SendGrid is Fun";
            //var to = new EmailAddress("test@example.com", "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                       

            contactService.CreateMail(contactModel);

            return RedirectToAction("All", "Bakery");
        }
    }
}
