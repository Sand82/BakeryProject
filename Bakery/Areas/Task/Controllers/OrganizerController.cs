using Bakery.Service.Organizers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Task.Controllers
{
    public class OrganizerController : AdminController
    {
        private readonly IOrganizerService organizerService;

        public OrganizerController(IOrganizerService organizerService)
        {
            this.organizerService = organizerService;
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        [Area(AreaNameTask)]        
        public IActionResult Request() 
        {
            var date = DateTime.UtcNow;            

            var items = organizerService.GetItems(date);

            return View(items); 
        }
    }
}
