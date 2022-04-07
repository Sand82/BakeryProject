﻿using Bakery.Service;
using Bakery.Models.Customer;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;
using static Bakery.WebConstants;

namespace Bakery.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;        
        private readonly ICustomerService customerService;

        public OrderController(IOrderService orderService,           
            ICustomerService customerService)
        {
            this.orderService = orderService;           
            this.customerService = customerService;
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

            var formCustomerOrder = orderService.GetCustomer(userId);

            if (formCustomerOrder == null)
            {
                formCustomerOrder = new CustomerFormModel();
            }

            formCustomerOrder.Order = orderModel;
            formCustomerOrder.OrderId = orderModel.Id;           
                       
            return View(formCustomerOrder);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Buy(CustomerFormModel formCustomerOrder)
        {            

            var actualDate = DateTime.UtcNow;

            string? stringDate = formCustomerOrder.Order.DateOfDelivery == null ? "00.00.0000" : formCustomerOrder.Order.DateOfDelivery.ToString();

            var (isValidDate, dateOfDelivery) = orderService.TryParceDate(stringDate);

            if (dateOfDelivery < actualDate)
            {
                ModelState.AddModelError(WebConstants.DateOfDelivery, "The date cannot be older than the current one.");
            }

            if (!isValidDate)
            {
                ModelState.AddModelError(WebConstants.DateOfDelivery, "Invalid date format.");
            }

            var userId = GetUserId();

            formCustomerOrder.UserId = userId;

            var order = orderService.FindOrderByUserId(userId);

            if (order.Items.Count() == 0)
            {
                this.TempData[EmptyOrder] = "Cannot complete empty order.";

                return RedirectToAction("All", "Bakery");
            }

            if (!ModelState.IsValid)
            {                
                var orderModel = orderService.CreateOrderModel(order);

                formCustomerOrder.Order.DateOfOrder = orderModel.DateOfOrder;

                formCustomerOrder = new CustomerFormModel
                {
                    Order = orderModel,
                    OrderId = orderModel.Id,
                };               

                return View(formCustomerOrder);
            }          
                   
            var finishedOrder = orderService.FinishOrder(order, dateOfDelivery);            
            
            var customer = customerService.CreateCustomer(userId, formCustomerOrder);           

            customer.Order = finishedOrder;

            customerService.AddCustomer(customer);

            this.TempData[SuccessOrder] = "Order added seccessfully.";

            return RedirectToAction("All", "Bakery");
        }        

        private string GetUserId()
        {
            return User.GetId();
        }     
        
    }
}
