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

        int FindOrderIdByUserId(string userId);

        CreateOrderModel CreateOrderModel(string userId);

        Order FinishOrder(Order order, DateTime dateOfOrder);
    }
}
