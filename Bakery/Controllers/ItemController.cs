using Bakery.Models.Items;
using Bakery.Service.Bakeries;
using Bakery.Service.Items;
using Bakery.Service.Orders;
using Bakery.Service.Votes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService itemsService;
        private readonly IVoteService voteService;
        private readonly IOrderService orderService;
        private readonly IBakerySevice bakerySevice;

        public ItemController(IItemService itemsService,
            IVoteService voteService,
            IOrderService orderService,
            IBakerySevice bakerySevice)
        {
            this.itemsService = itemsService;
            this.voteService = voteService;
            this.orderService = orderService;
            this.bakerySevice = bakerySevice;
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

        [Authorize]
        [HttpPost]
        public IActionResult Details(int id, int quantity)
        {
            if (quantity < 1 || quantity > 2000)
            {
                return Redirect("/Item/Details/" + id);
            }

            var dataProduct = bakerySevice.CreateNamePriceModel(id);

            if (dataProduct == null)
            {
                return BadRequest();
            }

            var ParsePrice = Decimal.TryParse(dataProduct.Price, out var currPrice);

            if (!ParsePrice)
            {
                throw new InvalidOperationException("Unknown format for 'Price'");
            }

            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            var order = orderService.FindOrderByUserId(userId);

            if (order == null)
            {
                order = orderService.CreatOrder(userId);
            }

            var item = orderService.CreateItem(id, dataProduct.Name, currPrice, quantity, userId);

            orderService.AddItemInOrder(item, order);

            return RedirectToAction("All", "Bakery");

        }

        [Authorize]
        public IActionResult Vote(int id, byte vote)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            if (vote < 0 || vote > 5)
            {
                return BadRequest();
            }

            var product = bakerySevice.FindById(id);

            if (product == null)
            {
                return BadRequest();
            }

            var userId = User.GetId();

            voteService.SetVote(userId, id, vote);

            return RedirectToAction("Details", "Item", new { id });
        }

        [Authorize]
        public IActionResult EditAll(int id)
        {
            List<EditItemsFormModel>? items = null;

            try
            {
                items = itemsService.GetAllItems(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex);
            }

            return View(items);
        }

        [Authorize]
        public IActionResult DeleteAll(int id)
        {
            var order = itemsService.FindOrderById(id);

            if (order == null)
            {
                return BadRequest();
            }

            itemsService.DeleteAllItems(order);

            return RedirectToAction("Buy", "Order");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = User.GetId();            

            var order = itemsService.FindOrderByUserId(userId);

            if (order == null)
            {
                return BadRequest();
            }

            var item = itemsService.FindItemById(id);

            if (item == null)
            {
                return BadRequest();
            }

            itemsService.DeleteItem(item, order);

            return RedirectToAction("Buy", "Order");
        }
    }
}
