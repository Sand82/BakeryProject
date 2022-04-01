﻿using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IOrderService orderService;
        private readonly IDetailsService detailsService;

        public ItemController(IItemsService itemsService, IOrderService orderService, IDetailsService detailsService)
        {
            this.itemsService = itemsService;
            this.orderService = orderService;
            this.detailsService = detailsService;
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

            if(id == 0 || quantity < 0 || quantity > 2000)
            {
                ModelState.AddModelError("Quantity", "Quantity is not valid value!");                
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Item");
            }

            var userId = User.GetId();

            var order = orderService.FindOrderByUserId(userId);

            if (order == null)
            {
                order = orderService.CreatOrder(userId);
            }

            var orderProduct = detailsService.AddProductToOrder(id, order.Id, quantity);

            return RedirectToAction("All", "Product");
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
