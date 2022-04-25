using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class ContactController : Controller
    {
        [Authorize]
        public IActionResult Location()
        {
            return View();
        }
    }
}
