using Bakery.Models.EditItem;
using Bakery.Service.Items;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EditItemController : Controller
    {
        private readonly IItemService itemsService;

        public EditItemController(IItemService itemsService)
        {
            this.itemsService = itemsService;
        }

        [HttpPost]
        [Authorize]        
        public void Post(EditItemDataModel model) 
        {
            try
            {
                itemsService.ChangeItemQuantity(model);
            }
            catch (Exception)
            {
                throw new NullReferenceException("Not found");
            }
        }        
    }
}
