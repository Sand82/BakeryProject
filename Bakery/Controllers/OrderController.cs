using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Add(int id)
        {


            return RedirectToAction("All", "Bakery");
        }

        public IActionResult Buy()
        {
            return View();
        }
    }
}
