using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Job.Controllers
{
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    [Area(AreaName)]
    public class EmployeeController : Controller
    {

        public IActionResult Approve()
        {

            return View();
        }
    }
}
