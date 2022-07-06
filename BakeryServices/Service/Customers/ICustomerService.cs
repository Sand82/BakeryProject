using BakeryData.Models;
using BakeryServices.Models.Customer;

namespace BakeryServices.Service.Customers
{
    public interface ICustomerService
    {
        Task<Customer> FindCustomer(string userId, CustomerFormModel formCustomerOrder);

        Customer CreateCustomer(string userId, CustomerFormModel formCustomerOrder);

        Task AddCustomer(Customer customer);
    }
}
