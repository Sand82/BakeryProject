using Bakery.Data;
using Bakery.Models.EditItem;
using Bakery.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EditItemController : Controller
    {
        private readonly ItemsService itemsService;
        private readonly IOrderService orderService;
        private readonly BackeryDbContext data;

        public EditItemController(ItemsService itemsService, IOrderService orderService, BackeryDbContext data)
        {
            this.itemsService = itemsService;
            this.orderService = orderService;
            this.data = data;
        }

        [HttpPost]
        [Authorize]
       
        public ActionResult Post(EditItemDataModel model) 
        {
            //var userId = User.GetId();

            //var order = orderService.FindOrderByUserId(userId);

            //if (order == null)
            //{
            //    return BadRequest();
            //}

            var item = itemsService.FindItemById(model.ItemId);

            item.Quantity = model.Quantity;

            this.data.SaveChanges();

            return RedirectToAction("Buy", "Order");
        }
    }
}
