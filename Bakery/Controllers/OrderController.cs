using Bakery.Service;
using Bakery.Models.Customer;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IItemsService itemsService;
        private readonly ICustomerService customerService;

        public OrderController(IOrderService orderService,
            IItemsService itemsService,
            ICustomerService customerService)
        {
            this.orderService = orderService;
            this.itemsService = itemsService;
            this.customerService = customerService;
        }
        
        //[Authorize]
        //public IActionResult Add(int id, string name, string price, int quantity)
        //{
        //    if (quantity < 1 || quantity > 2000)
        //    {
        //        return Redirect("/Item/Details/" + id);
        //    }

        //    var ParsePrice = Decimal.TryParse(price, out var currPrice);

        //    if (!ParsePrice)
        //    {
        //        throw new InvalidOperationException("Unknown format for 'Price'");
        //    }

        //    var userId = GetUserId();

        //    if (userId == null)
        //    {
        //        return BadRequest();
        //    }

        //    var order = orderService.FindOrderByUserId(userId);

        //    if (order == null)
        //    {
        //       order = orderService.CreatOrder(userId);
        //    }

        //    var item = itemsService.FindItem(name, quantity, currPrice);            
            

        //    if (item == null )
        //    {
        //        item = orderService.CreateItem(id, name, currPrice, quantity, userId);
        //    }

        //    orderService.AddItemInOrder(item, order);

        //    return RedirectToAction("All", "Bakery");
        //} 
        

        [Authorize]
        public IActionResult Buy()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return BadRequest();
            }           

            var orderModel = orderService.CreateOrderModel(userId);

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
            var actualDate = DateTime.UtcNow;

            var (isValidDate, dateOfDelivery) = TryParceDate(formCustomerOrder.Order.DateOfDelivery.ToString());

            if (dateOfDelivery < actualDate)
            {
                ModelState.AddModelError(WebConstants.DateOfDelivery, "The date cannot be older than the current one.");
            }

            var userId = GetUserId();

            formCustomerOrder.UserId = userId;

            var order = orderService.FindOrderByUserId(userId);

            if (!ModelState.IsValid)
            {                
                var orderModel = orderService.CreateOrderModel(userId);

                formCustomerOrder.Order.DateOfOrder = orderModel.DateOfOrder;

                formCustomerOrder = new CustomerFormModel
                {
                    Order = orderModel,
                    OrderId = orderModel.Id,
                };               

                return View(formCustomerOrder);
            }          
                      
            if (!isValidDate)
            {
                return BadRequest();
            }

            var finishedOrder = orderService.FinishOrder(order, dateOfDelivery);            
            
            var customer = customerService.CreateCustomer(userId, formCustomerOrder);           

            customer.Order = finishedOrder;

            customerService.AddCustomer(customer);

            return RedirectToAction("All", "Bakery");
        }

        private string GetUserId()
        {
            return User.GetId();
        }  
        
        private (bool, DateTime) TryParceDate(string date)
        {         
            DateTime dateOfOrder;

           var isValidDate = DateTime.TryParseExact(
               date.ToString(),
               "dd.MM.yyyy", CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out dateOfOrder);

            return (isValidDate, dateOfOrder);
        }
    }
}
