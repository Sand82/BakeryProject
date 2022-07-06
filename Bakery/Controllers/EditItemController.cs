using BakeryServices.Models.EditItem;
using BakeryServices.Service.Items;

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
        public async Task Post(EditItemDataModel model) 
        {
            try
            {
               await itemsService.ChangeItemQuantity(model);
            }
            catch (Exception)
            {
                throw new NullReferenceException("Not found");
            }
        }        
    }
}
