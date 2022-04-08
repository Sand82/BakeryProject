using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Service;
using Bakery.Tests.Mock;

using System;
using System.Collections.Generic;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Services
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void GetEmployeeByIdShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            SaveEmployeeCollectionInDatabase(data);

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetEmployeeById(1);

            Assert.NotNull(result);            
        }

        [Fact]
        public void GetEmployeeByIdShouldReturnNullWithIncorectMethodParameters()
        {
            using var data = DatabaseMock.Instance;

            SaveEmployeeCollectionInDatabase(data);

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetEmployeeById(10);

            Assert.Null(result);
        }

        [Fact]
        public void GetModelByIdShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            SaveEmployeeCollectionInDatabase(data);

            var employeeService = new EmployeeService(data);

            var model = new EmployeeInfoViewModel
            {
                Id = 0,
                Name = $"Employee1",
                Description = $"GoodEmployee1",
                Experience = "1",
                Age = 31,
                FilePath = "/files/1.txt",
                Image = "/files/1.png"
            };

            var result = employeeService.GetModelById(1);

            var obj1 = ConvertToJason(result);
            var obj2 = ConvertToJason(model);

            Assert.NotNull(result);
            Assert.Equal(obj2,obj1);
        }

        [Fact]
        public void GetModelByIdShouldReturnNullWithIncorectMethodParameters()
        {
            using var data = DatabaseMock.Instance;

            SaveEmployeeCollectionInDatabase(data);

            var employeeService = new EmployeeService(data);            

            var ex = Assert.Throws<System.NullReferenceException>(() => employeeService.GetModelById(10));

            Assert.Equal("Not Found", ex.Message);
        }

        private void SaveEmployeeCollectionInDatabase(BakeryDbContext data)
        {

            var employess = CreateListEmployees();

            data.Employees.AddRange(employess);

            data.SaveChanges();            
        }

        private List<Employee> CreateListEmployees()
        {
            var employees = new List<Employee>();

            for (int i = 1; i <= 5; i++)
            {
                var employee = new Employee
                {
                    Id = i,
                    FullName = $"Employee{i}",
                    Age = 30 + i,
                    Phone = $"+35988877765{i}",
                    Description = $"GoodEmployee{i}",
                    Experience = $"{i}",
                    ApplayDate = DateTime.Now,
                    Email = $"test{i}@abv.bg",
                    FileExtension = "txt",
                    ImageExtension = "png",
                    FileId = $"{i}",
                    ImageId = $"{i}",
                    IsApproved = null,
                };

                employees.Add(employee);
            }           

            return employees;
        }
    }
}
