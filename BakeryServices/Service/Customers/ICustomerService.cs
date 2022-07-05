using Bakery.Data.Models;
using Bakery.Models.Customer;

namespace Bakery.Service.Customers
{
    public interface ICustomerService
    {
        Task<Customer> FindCustomer(string userId, CustomerFormModel formCustomerOrder);

        Customer CreateCustomer(string userId, CustomerFormModel formCustomerOrder);

        Task AddCustomer(Customer customer);
    }
}
