﻿using BakeryServices.Models.Organaizers;
using BakeryServices.Service.Orders;
using BakeryServices.Service.Organizers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Areas.Task.Controllers
{    
    [ApiController]
    [Route("Task/api/[controller]")]
    public class ProfitController : AdminController
    {
        private readonly IOrderService orderService;
        private readonly IOrganizerService organizerService;

        public ProfitController(IOrderService orderService, IOrganizerService organizerService)
        {
            this.orderService = orderService;
            this.organizerService = organizerService;
        }

        [Authorize]
        [HttpPost]        
        public async Task<string> Check(CheckFormModel model)
        {
           
           var (IsValidFromDate, fromDate) = orderService.TryParceDate(model.ValueFrom);

           var (IsValidToDate, toDate) = orderService.TryParceDate(model.ValueTo);

            if (!IsValidFromDate || !IsValidToDate || DateTime.Compare(fromDate, toDate) > 0)
            {
                return $"Invalid data format or data period";
            }

            var totallProfit = await organizerService.GetCustomProfit(fromDate, toDate);            

            return totallProfit;
        }
    }
}
