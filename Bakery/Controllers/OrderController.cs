using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Bakery.Models;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;
using Bakery.Service;
using Bakery.Data;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly BackeryDbContext data;

        public OrderController(IOrderService orderService, BackeryDbContext data)
        {
            this.orderService = orderService;

            this.data = data;
        }
        
        [Authorize]
        public IActionResult Add(int id, string name, string price, int quantity)
        {
            if (quantity < 1 || quantity > 2000)
            {
                return Redirect("/Item/Details/" + id);
            }

            var ParsePrice = Decimal.TryParse(price, out var currPrice);

            if (!ParsePrice)
            {
                throw new InvalidOperationException("Unknown format for 'Price'");
            }

            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            var order = this.data.Orders.FirstOrDefault(o => o.UserId == userId);

            if (order == null)
            {
               order = orderService.CreatOrder(userId);
            }

            var item = this.data.Items
                .FirstOrDefault(i => i.ProductName == name && i.Quantity == quantity && i.ProductPrice == currPrice);

            if (item == null )
            {
                item = orderService.CreateItem(id, name, currPrice, quantity, userId);
            }

            orderService.AddItemInOrder(item, order);

            return RedirectToAction("All", "Bakery");
        } 
        

        [Authorize]
        public IActionResult Buy()
        {
            return View();
        }
    }
}
