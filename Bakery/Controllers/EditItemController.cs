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

        public EditItemController(IItemsService itemsService)
        {
            this.itemsService = itemsService;
        }

        [HttpPost]
        [Authorize]        
        public void Post(EditItemDataModel model) 
        {
            itemsService.ChangeItemQuantity(model);         
            
        }        
    }
}
