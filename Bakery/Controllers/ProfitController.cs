using Bakery.Areas.Task.Models;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ProfitController : Controller
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

            if (!IsValidFromDate || !IsValidFromDate || DateTime.Compare(fromDate, toDate) > 0)
            {
                
            }

            return null;
        }
    }
}
