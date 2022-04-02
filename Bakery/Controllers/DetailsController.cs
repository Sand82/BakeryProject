using Bakery.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : Controller
    {

        [HttpPost]
        [Authorize]       
        public IActionResult Post(DetailsModel details)
        {
            if(!ModelState.IsValid)
            {
                return View(details);
            }

            return RedirectToAction();
        }
    }
}
