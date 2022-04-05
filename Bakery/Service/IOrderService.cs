using Bakery.Data.Models;
using Bakery.Models.Customer;
using Bakery.Models.Orders;

namespace Bakery.Service
{
    public interface IOrderService
    {
        Order CreatOrder(string userId);

        Item CreateItem(int id, string name, decimal price, int quantity, string userId);

        void AddItemInOrder(Item item, Order order);

        Order FindOrderByUserId(string userId);

        CreateOrderModel CreateOrderModel(Order order);

        Order FinishOrder(Order order, DateTime dateOfOrder);

        (bool, DateTime) TryParceDate(string date);

        CustomerFormModel GetCustomer(string userId);
    }
}
