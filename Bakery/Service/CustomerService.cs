using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Customer;

namespace Bakery.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly BackeryDbContext data;

        public CustomerService(BackeryDbContext data)
        {
            this.data = data;
        }       

        public Customer CreateCustomer(string userId, CustomerFormModel form)
        {
            var customer = new Customer
            {
                UserId = userId,
                FirstName = form.FirstName,
                LastName = form.LastName,
                PhoneNumber = form.PhoneNumber,
                Adress = form.Address,
                Email = form.Email,
            };

            return customer;
        }

        public Customer FindCustomer(string userId, CustomerFormModel form)
        {
            var customer = this.data
                .Customers
                .Where(x => x.UserId == userId &&
                    x.FirstName == form.FirstName && 
                    x.LastName == form.LastName &&
                    x.PhoneNumber == form.PhoneNumber &&
                    x.Email == form.Email &&
                    x.Adress == form.Address)                
                .FirstOrDefault();

            return customer;
        }

        public void AddCustomer(Customer customer)
        {
            this.data.Customers.Add(customer);

            this.data.SaveChanges();
        }
    }
}
