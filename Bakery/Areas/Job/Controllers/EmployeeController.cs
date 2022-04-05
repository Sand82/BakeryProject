using Bakery.Areas.Job.Models;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Job.Controllers
{    
    public class EmployeeController : AdminController
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
            var employee = employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return BadRequest();
            }

            employeeService.SetEmployee(employee, true);

            return RedirectToAction("Approve", "Employee");
        }


        [Authorize]
        public IActionResult Reject(int id)
        {
            var employee = employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return BadRequest();
            }

            employeeService.SetEmployee(employee, false);

            return RedirectToAction("Approve", "Employee");
        }

        [Authorize]
        public IActionResult Info(int id)
        {
            var employee = employeeService.GetModelById(id);

            if (employee == null)
            {
                return BadRequest();
            }

            return View(employee);
        }        
    }
}
