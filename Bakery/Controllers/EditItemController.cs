using Bakery.Data;
using Bakery.Models.EditItem;
using Bakery.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EditItemController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IOrderService orderService;
        private readonly BackeryDbContext data;

        public EditItemController(IItemsService itemsService, IOrderService orderService, BackeryDbContext data)
        {
            this.itemsService = itemsService;
            this.orderService = orderService;
            this.data = data;
        }

        [HttpPost]
        [Authorize]        
        public void Post(EditItemDataModel model) 
        {

            itemsService.ChangeItemQuantity(model);           
            
        }
    }
}
