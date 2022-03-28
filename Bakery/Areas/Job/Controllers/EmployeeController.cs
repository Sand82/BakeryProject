using Bakery.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Areas.Job.Controllers
{
    public class EmployeeController : AdminController
    {

        public IActionResult Applay()
        {

            return View();
        }
    }
}
