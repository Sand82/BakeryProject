using Bakery.Areas.Job.Models;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Job.Controllers
{
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    [Area(AreaName)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [Authorize]
        public IActionResult Approve()
        {
            var employees = employeeService.GetAllApplies();

            return View(employees);
        }
       
        [Authorize]
        public IActionResult Add(int id)
        {
            var employee = employeeService.GetById(id);

            if (employee == null)
            {
                return BadRequest();
            }

            employeeService.SetEmployee(employee);

            return RedirectToAction("Home", "Index");
        }
    }
}
