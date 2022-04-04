using Bakery.Areas.Task.Models;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Areas.Task.Controllers
{    
    [ApiController]
    [Route("Task/api/[controller]")]
    public class ProfitController : AdminController
    {
        private readonly IOrderService orderService;
        

        public ProfitController(IOrderService orderService)
        {
            this.orderService = orderService;            
        }

        [Authorize]        
        public string Check(CheckFormModel model)
        {
           
           var (IsValidFromDate, fromDate) = orderService.TryParceDate(model.ValueFrom);

           var (IsValidToDate, toDate) = orderService.TryParceDate(model.ValueTo);

            if (!IsValidFromDate || !IsValidToDate || DateTime.Compare(fromDate, toDate) > 0)
            {
                
            }

            return null;
        }
    }
}
