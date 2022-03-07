using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Items;
using Bakery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemsService itemsService;

        public ItemController(IItemsService itemsService)
        {
            this.itemsService = itemsService;
        }
        public IActionResult Details(int id)
        {
            var product = itemsService.GetDetails(id);

            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }
    }
}
