using Bakery.Areas.Job.Models;
using Bakery.Service.Employees;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            EmployeeInfoViewModel employee = null;

            try
            {
                employee = employeeService.GetModelById(id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex);
            }            

            if (employee == null)
            {
                return BadRequest();
            }

            return View(employee);
        }        
    }
}
