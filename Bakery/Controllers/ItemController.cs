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

        public ItemController(IItemsService itemsService, IVoteService voteService)
        {
            this.itemsService = itemsService;
            this.voteService = voteService;            
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var product = itemsService.GetDetails(id);

            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        [Authorize]
        public IActionResult Vote(int id, byte vote)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (vote < 0 || vote > 5)
            {
                return BadRequest();
            }

            var userId = User.GetId();

            voteService.SetVote(userId, id, vote);         
            
            return RedirectToAction("Details", "Item",  new { id });
        }
    }
}
