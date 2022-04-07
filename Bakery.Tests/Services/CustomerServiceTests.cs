using Bakery.Data.Models;
using Bakery.Models.Customer;
using Bakery.Service;
using Bakery.Tests.Mock;

using Xunit;

namespace Bakery.Tests.Services
{
    public class CustomerServiceTests
    {
        [Fact]
        public void CreateCustomerShoudReturnNewCustomerWhenModelIsValid()
        {
            var userId = "SomeUser";

            var form = ReturnModel();

            var customerService = new CustomerService(null);

            var result = customerService.CreateCustomer(userId, form);

            Assert.NotNull(result);
        }

        [Fact]
        public void FindCustomerShoudReturnCustomer()
        {
            using var data = DatabaseMock.Instance;

            var userId = "SomeUser";

            data.Customers.Add(new Customer
            {
                UserId = userId,
                FirstName = "Sand",
                LastName = "Stefanov",
                Email = "Sand@abv.bg",
                Adress = "Vraca,ul.Svoboda ,blok.34",
                PhoneNumber = "+359888777666",
            });    
            
            data.SaveChanges();

            var form = ReturnModel();

            var customerService = new CustomerService(data);

            var result = customerService.FindCustomer(userId, form);

            Assert.NotNull(result);

        }

        [Fact]        
        public void FindCustomerShoudReturnNullWithDiferentModel()
        {
            using var data = DatabaseMock.Instance;

            var userId = "SomeUser";

            data.Customers.Add(new Customer
            {
                UserId = userId,
                FirstName = "Sand",
                LastName = "Stefanov",
                Email = "Sand@abv.bg",
                Adress = "Vraca,ul.Svoboda ,blok.34",
                PhoneNumber = "+359888777666",
            });

            data.SaveChanges();

            var form = new CustomerFormModel
            {
                UserId = userId,
                FirstName = "Sand82",
                LastName = "Stefanov",
                Email = "Sand@abv.bg",
                Address = "Vraca,ul.Svoboda ,blok.34",
                PhoneNumber = "+359888777666",
            };

            var customerService = new CustomerService(data);

            var result = customerService.FindCustomer(userId, form);

            Assert.Null(result);           
        }

        private CustomerFormModel ReturnModel() 
        {
            var userId = "SomeUser";

            var model = new CustomerFormModel
            {
                UserId = userId,
                FirstName = "Sand",
                LastName = "Stefanov",
                Email = "Sand@abv.bg",
                Address = "Vraca,ul.Svoboda ,blok.34",
                PhoneNumber = "+359888777666",
            };

            return model;
        }
    }
}
