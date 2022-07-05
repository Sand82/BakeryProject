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
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.GetId();

            var product = await itemsService.GetDetails(id, userId);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(int id, int quantity)
        {
            if (quantity < 1 || quantity > 2000)
            {
                return Redirect("/Item/Details/" + id);
            }

            var dataProduct =await bakerySevice.CreateNamePriceModel(id);

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

            var order = await orderService.FindOrderByUserId(userId);

            if (order == null)
            {
                order = await orderService.CreatOrder(userId);
            }

            var item = await orderService.CreateItem(id, dataProduct.Name, currPrice, quantity, userId);

            await orderService.AddItemInOrder(item, order);

            return RedirectToAction("All", "Bakery");

        }

        [Authorize]
        public async Task<IActionResult> Vote(int id, byte vote)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            if (vote < 0 || vote > 5)
            {
                return BadRequest();
            }

            var product = await bakerySevice.FindById(id);

            if (product == null)
            {
                return BadRequest();
            }

            var userId = User.GetId();

            await voteService.SetVote(userId, id, vote);

            return RedirectToAction("Details", "Item", new { id });
        }

        [Authorize]
        public async Task<IActionResult> EditAll(int id)
        {
            List<EditItemsFormModel>? items = null;

            try
            {
                items = await itemsService.GetAllItems(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex);
            }

            return View(items);
        }

        [Authorize]
        public async Task<IActionResult> DeleteAll(int id)
        {
            var order = await itemsService.FindOrderById(id);

            if (order == null)
            {
                return BadRequest();
            }

            await itemsService.DeleteAllItems(order);

            return RedirectToAction("Buy", "Order");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetId();            

            var order = await itemsService.FindOrderByUserId(userId);

            if (order == null)
            {
                return BadRequest();
            }

            var item = await itemsService.FindItemById(id);

            if (item == null)
            {
                return BadRequest();
            }

            await itemsService.DeleteItem(item, order);

            return RedirectToAction("Buy", "Order");
        }
    }
}
