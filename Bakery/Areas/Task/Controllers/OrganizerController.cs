﻿using BakeryServices.Service.Organizers;

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
        public async Task<IActionResult> Requests() 
        {
            var date = DateTime.UtcNow;            

            var items = await organizerService.GetItems(date);

            return View(items); 
        }
    }
}
