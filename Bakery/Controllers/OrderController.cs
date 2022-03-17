using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Buy(int id)
        {
            return View();
        }
    }
}
