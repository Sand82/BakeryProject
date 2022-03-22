using Bakery.Data.Models;
using Bakery.Models;
using Bakery.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        
        [Authorize]
        public IActionResult Add(int id, string name, string price, int quantity)
        {
            if (quantity < 1 || quantity > 2000)
            {
                return Redirect("/Item/Details/" + id);
            }

           return RedirectToAction("All", "Bakery");
        } 
        

        [Authorize]
        public IActionResult Buy()
        {
            return View();
        }
    }
}
