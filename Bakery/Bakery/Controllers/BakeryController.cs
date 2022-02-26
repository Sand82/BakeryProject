using Bakery.Models.Bakery;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class BakeryController : Controller
    {
        public IActionResult Add()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Add(BakeryAddFormModel bakeProduct)
        {
            return View();
        }
    }
}
