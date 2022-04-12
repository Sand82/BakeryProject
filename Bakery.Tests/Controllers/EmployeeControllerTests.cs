using Bakery.Areas.Job.Controllers;
using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Service;
using Bakery.Tests.Mock;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace Bakery.Tests.Controllers
{
    public class EmployeeControllerTests
    {

        [Fact]
        public void ApproveShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var controller = new EmployeeController(employeeService);

            var result = controller.Approve();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            AddEmployeesInDatabase(data);

            var employeeService = new EmployeeService(data);

            var controller = new EmployeeController(employeeService);

            var result = controller.Add(1);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);                
            
            Assert.Equal("Approve", viewResult.ActionName);
            Assert.Equal("Employee", viewResult.ControllerName);
        }

        [Fact]
        public void AddShouldReturnBadRequestWhenEmployeeIsNotFound()
        {
            using var data = DatabaseMock.Instance;

            AddEmployeesInDatabase(data);

            var employeeService = new EmployeeService(data);

            var controller = new EmployeeController(employeeService);

            var result = controller.Add(10);           

            var viewResult = Assert.IsType<BadRequestResult>(result);

            Assert.True(viewResult.StatusCode == 400);
        }

        [Fact]
        public void RejectShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            AddEmployeesInDatabase(data);

            var employeeService = new EmployeeService(data);

            var controller = new EmployeeController(employeeService);

            var result = controller.Reject(1);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Approve", viewResult.ActionName);
            Assert.Equal("Employee", viewResult.ControllerName);
        }

        [Fact]
        public void RejectShouldReturnBadRequestWhenEmployeeIsNotFound()
        {
            using var data = DatabaseMock.Instance;

            AddEmployeesInDatabase(data);

            var employeeService = new EmployeeService(data);

            var controller = new EmployeeController(employeeService);

            var result = controller.Reject(10);

            var viewResult = Assert.IsType<BadRequestResult>(result);

            Assert.True(viewResult.StatusCode == 400);
        }

        private void AddEmployeesInDatabase(BakeryDbContext data)
        {
            var employees = CreateEmployees();

            data.Employees.AddRange(employees);

            data.SaveChanges();
        }

        private List<Employee> CreateEmployees()
        {
            var employees = new List<Employee>();

            for (int i = 1; i < 6; i++)
            {
                var employee = new Employee
                {
                    Id = i,
                    FullName = $"Employee{i}",
                    Age = 25 + i,
                    Description = "good employee",
                    Email = $"employee{i}@abv.bg",
                    Phone = $"+35988865432{i}",
                    Experience = $"{2 + i} years",
                    FileExtension = "doc",
                    FileId = $"File{i}",
                    ImageExtension = "png",
                    ImageId = $"Image{i}",
                    IsApproved = null,
                    ApplayDate = System.DateTime.UtcNow,                    
                };
                employees.Add(employee);
            };

            return employees;
        }
    }
}
