using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IVoteService voteService;
        private readonly IOrderService orderService;

        public ItemController(IItemsService itemsService, IVoteService voteService, IOrderService orderService)
        {
            this.itemsService = itemsService;
            this.voteService = voteService;
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var userId = User.GetId();

            var product = itemsService.GetDetails(id, userId);

            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        //[Authorize]
        //public IActionResult Vote(int id, byte vote)
        //{
        //    if (id == 0)
        //    {
        //        return BadRequest();
        //    }

        //    if (vote < 0 || vote > 5)
        //    {
        //        return BadRequest();
        //    }

        //    var userId = User.GetId();

        //    voteService.SetVote(userId, id, vote);         
            
        //    return RedirectToAction("Details", "Item",  new { id });
        //}

        [Authorize]
        public IActionResult EditAll(int id)
        {
            var items = itemsService.GetAllItems(id);
                        
            return View(items);
        }

        [Authorize]
        public IActionResult DeleteAll (int id)
        {
             var order = itemsService.FindOrderById(id);

             itemsService.DeleteAllItems(order);

            return RedirectToAction("Buy", "Order");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = User.GetId();

            var order = itemsService.FindOrderByUserId(userId);   
            
            var item = itemsService.FindItemById(id);

            itemsService.DeleteItem(item, order);

            return RedirectToAction("Buy", "Order");
        }       
    }
}
