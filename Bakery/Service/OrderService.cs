using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models;

namespace Bakery.Service
{
    public class OrderService : IOrderService
    {
        private readonly BackeryDbContext data;

        public OrderService(BackeryDbContext data)
        {
            this.data = data;
        }

        public Order CreatOrder(string userId)
        {
            var order = new Order
            {
                UserId = userId,
            };

            this.data.Orders.Add(order);

            this.data.SaveChanges();

            return order;
        }

        public Item CreateItem(int id, string name, decimal price, int quantity, string userId)
        {           
                      
            var item = new Item
            {                
                ProductName = name,
                ProductPrice = price,
                Quantity = quantity,               
            };

            this.data.Items.Add(item);

            this.data.SaveChanges();

            return item;
        }

        public void AddItemInOrder(Item item, Order order)
        {
            order.Items.Add(item);

            this.data.SaveChanges();
        }
    }
}
