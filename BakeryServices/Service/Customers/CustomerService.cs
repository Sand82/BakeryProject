using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Customer;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Service.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly BakeryDbContext data;

        public CustomerService(BakeryDbContext data)
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

        public async Task<Customer> FindCustomer(string userId, CustomerFormModel form)
        {
            var customer = await this.data
            .Customers
            .Where(x => x.UserId == userId &&
                x.FirstName == form.FirstName &&
                x.LastName == form.LastName &&
                x.PhoneNumber == form.PhoneNumber &&
                x.Email == form.Email &&
                x.Adress == form.Address)
            .FirstOrDefaultAsync();

            return customer;
        }

        public async Task AddCustomer(Customer customer)
        {
            await this.data.Customers.AddAsync(customer);

            await this.data.SaveChangesAsync();
        }
    }
}
