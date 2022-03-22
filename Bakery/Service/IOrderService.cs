using Bakery.Data.Models;
using Bakery.Models;

namespace Bakery.Service
{
    public interface IOrderService
    {
        Order CreatOrder(string userId);

        Item CreateItem(int id, string name, decimal price, int quantity, string userId);

        void AddItemInOrder(Item item, Order order);
    }
}
