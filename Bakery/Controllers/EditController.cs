using Bakery.Models.EditItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditItemController : Controller
    {
        [HttpPost]
        [Authorize]
        [IgnoreAntiforgeryToken]
        public ActionResult Post(EditItemDataModel model) 
        {
            return RedirectToAction("");
        }
    }
}
