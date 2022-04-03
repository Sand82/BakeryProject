using Microsoft.AspNetCore.Mvc;

namespace Bakery.Areas.Task.Controllers
{
    public class OrganizerController : Controller
    {
        public IActionResult Request() 
        { 
            return View(); 
        }
    }
}
