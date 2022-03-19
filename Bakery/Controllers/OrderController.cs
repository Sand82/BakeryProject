using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        [Authorize]
        public IActionResult Add(int id)
        {


            return RedirectToAction("All", "Bakery");
        }
        [Authorize]
        public IActionResult Buy()
        {
            return View();
        }
    }
}
