using BakeryServices.Models.Employees;
using BakeryServices.Service.Employees;

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
        public async Task<IActionResult> Approve()
        {
            var employees = await employeeService.GetAllApplies();

            return View(employees);
        }

        [Authorize]
        public async Task<IActionResult> Add(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return BadRequest();
            }

           await employeeService.SetEmployee(employee, true);

            return RedirectToAction("Approve", "Employee");
        }


        [Authorize]
        public async Task<IActionResult> Reject(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return BadRequest();
            }

            await employeeService.SetEmployee(employee, false);

            return RedirectToAction("Approve", "Employee");
        }

        [Authorize]
        public async Task<IActionResult> Info(int id)
        {
            EmployeeInfoViewModel employee = null;

            try
            {
                employee = await employeeService.GetModelById(id);
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
