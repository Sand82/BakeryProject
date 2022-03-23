using Bakery.Data.Models;
using Bakery.Models.Order;

namespace Bakery.Service
{
    public interface IOrderService
    {
        Order CreatOrder(string userId);

        Item CreateItem(int id, string name, decimal price, int quantity, string userId);

        void AddItemInOrder(Item item, Order order);

        Order FindOrderByUserId(string userId);

        CreateOrderModel CreateOrderModel(Order order);
    }
}
