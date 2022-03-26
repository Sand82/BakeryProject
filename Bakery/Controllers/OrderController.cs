using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Bakery.Models;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;
using Bakery.Service;
using Bakery.Data;
using Bakery.Models.Customer;
using System.Globalization;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IItemsService itemsService;

        public OrderController(IOrderService orderService, IItemsService itemsService)
        {
            this.orderService = orderService;
            this.itemsService = itemsService;
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

            var userId = GetUserId();

            if (userId == null)
            {
                return BadRequest();
            }

            var order = orderService.FindOrderByUserId(userId);

            if (order == null)
            {
               order = orderService.CreatOrder(userId);
            }

            var item = itemsService.FindItem(name, quantity, currPrice);            
            

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
            var userId = GetUserId();

            if (userId == null)
            {
                return BadRequest();
            }

            var order = orderService.FindOrderByUserId(userId);

            if (order == null)
            {
                order = orderService.CreatOrder(userId);
            }

            var orderModel = orderService.CreateOrderModel(order);

            var formCustomerOrder = new CustomerFormModel 
            {
                Order = orderModel,
                OrderId = orderModel.Id,                        
            };

            return View(formCustomerOrder);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Buy(CustomerFormModel formCustomerOrder)
        {
            var userId = GetUserId();

            formCustomerOrder.UserId = userId;            

            if (!ModelState.IsValid)
            {
                var order = orderService.FindOrderByUserId(userId);

                var orderModel = orderService.CreateOrderModel(order);

                formCustomerOrder.Order.DateOfOrder = orderModel.DateOfOrder;

                formCustomerOrder = new CustomerFormModel
                {
                    Order = orderModel,
                    OrderId = orderModel.Id,
                };

                return View(formCustomerOrder);
            }

            var (isValidDate, dateOfOrder) = TryParceDate(formCustomerOrder.Order.DateOfDelivery.ToString());

            
            if (!isValidDate)
            {
                return BadRequest();
            }

            orderService.FinishOrder(userId, dateOfOrder);

            return RedirectToAction("All", "Bakery");
        }

        private string GetUserId()
        {
            return User.GetId();
        }  
        
        private (bool, DateTime) TryParceDate(string date)
        {
            var isValidDate = true;

            DateTime dateOfOrder;

            isValidDate = DateTime.TryParseExact(
               date.ToString(),
               "dd.mm.yyyy", CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dateOfOrder);

            return (isValidDate, dateOfOrder);
        }
    }
}
