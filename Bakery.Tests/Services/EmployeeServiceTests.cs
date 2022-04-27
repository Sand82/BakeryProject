using Bakery.Areas.Job.Models;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Service.Employees;
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
            Assert.Equal(obj2, obj1);
        }

        [Fact]
        public void GetExstentionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetExstention("1", "txt");

            var expected = $"/files/1.txt";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1", null)]
        [InlineData(null, "txt")]
        public void GetExstentionShouldReturnInCorectResultWithIncorectMethodParameters(string id, string exstension)
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetExstention(id, exstension);

            var expected = $"/files/1.txt";

            Assert.False(result.Equals(expected));
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

        [Theory]
        [InlineData("/files/1.txt")]
        public void GetContentTypeShouldReturnCorectResultTest1(string path)
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetContentType(path);

            var expected = "text/plain";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/files/1.pdf")]
        public void GetContentTypeShouldReturnCorectResultTest2(string path)
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetContentType(path);

            var expected = "application/pdf";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/files/1.doc")]
        [InlineData("/files/1.docx")]
        public void GetContentTypeShouldReturnCorectResultTest3(string path)
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetContentType(path);

            var expected = "application/vnd.ms-word";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/files/1.odt")]
        public void GetContentTypeShouldReturnCorectResultTest4(string path)
        {
            using var data = DatabaseMock.Instance;

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetContentType(path);

            var expected = "application/vnd.oasis.opendocument.text";

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetContentTypeShouldReturnNullWithIncorectMethodParameters()
        {
            using var data = DatabaseMock.Instance;

            SaveEmployeeCollectionInDatabase(data);

            var employeeService = new EmployeeService(data);

            var ex = Assert.Throws<KeyNotFoundException>(() => employeeService.GetContentType("/files/1.spa"));

            Assert.Equal("Not supported file extension.", ex.Message);
        }


        [Fact]
        public void GetAllAppliesShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var employess = CreateListEmployees();

            employess[0].IsApproved = true;
            employess[1].IsApproved = false;

            data.Employees.AddRange(employess);

            data.SaveChanges();

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetAllApplies();           

            Assert.NotNull(result);
            Assert.StrictEqual(3, result.Count);            
        }

        [Fact]
        public void GetAllAppliesShouldReturnNullWhenDatabaseIsEmplty()
        {
            using var data = DatabaseMock.Instance;            

            var employeeService = new EmployeeService(data);

            var result = employeeService.GetAllApplies();
            
            Assert.StrictEqual(0, result.Count);
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
