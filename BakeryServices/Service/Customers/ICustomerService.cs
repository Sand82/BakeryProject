using Bakery.Data.Models;
using Bakery.Models.Customer;

namespace Bakery.Service.Customers
{
    public interface ICustomerService
    {
        Customer FindCustomer(string userId, CustomerFormModel formCustomerOrder);

        Customer CreateCustomer(string userId, CustomerFormModel formCustomerOrder);

        void AddCustomer(Customer customer);


    }
}
