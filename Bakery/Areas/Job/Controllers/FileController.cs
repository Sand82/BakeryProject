using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Areas.Job.Controllers{
    
    public class FileController : AdminController
    {
        private readonly IEmployeeService employeeService;

        public FileController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [Authorize]      
        public IActionResult Download(string filePath)
        {
            string contentType = null;

            try
            {
                contentType = employeeService.GetContentType(filePath);
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex);
            }            

            return File(filePath, contentType);
        }
    }
}
