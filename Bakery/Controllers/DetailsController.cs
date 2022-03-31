using Bakery.Models.Items;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;
using static Bakery.WebConstants;

namespace Bakery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DetailsController : Controller
    {
        private readonly IOrderService orderService;        

        public DetailsController(IOrderService orderService)
        {
            this.orderService = orderService;           
        }

        [HttpPost]
        [Authorize]       
        public IActionResult Details(DetailsModel details)
        {
            if(!ModelState.IsValid)
            {
                return View(details);
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
            
            var  item = orderService.CreateItem(details.ProductId, details.Quantity, userId);

            orderService.AddItemInOrder(item, order);

            this.TempData[ItemAdded] = "Item added seccessfully."; 

            return RedirectToAction("All", "Bakery");            
        }
    }
}
