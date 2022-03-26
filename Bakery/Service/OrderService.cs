using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models;
using Bakery.Models.Customer;
using Bakery.Models.Orders;
using Microsoft.EntityFrameworkCore;

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

            AddOrder(order);

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

            AddItem(item);

            return item;
        }

        public void AddItemInOrder(Item item, Order order)
        {
            order.Items.Add(item);

            this.data.SaveChanges();
        }       

        public Order FindOrderByUserId(string userId)
        {
            var order = this.data
                .Orders
                .Include(i => i.Items)
                .Where(o => o.UserId == userId && o.IsFinished == false && o.DateOfOrder.Day >= DateTime.UtcNow.Date.Day - 1)
                .OrderByDescending(i => i.Id)                               
                .FirstOrDefault();

            return order;
        }

        public CreateOrderModel CreateOrderModel(Order order)
        {
            var orderModel = new CreateOrderModel
            {
                Id = order.Id,
                IsFinished = order.IsFinished,
                DateOfOrder = order.DateOfOrder.ToString("dd.mm.yyyy")
            };

            var totalPrice = 0.0m;            

            foreach (var item in order.Items)
            {
                totalPrice += item.ProductPrice * item.Quantity;

                var newItem = new ItemFormViewModel
                {                    
                    Name = item.ProductName,
                    Price = item.ProductPrice.ToString("f2"),
                    Quantity = item.Quantity,
                };

                orderModel.items.Add(newItem);
            }

            orderModel.TotallPrice = totalPrice.ToString("f2");

            orderModel.ItemsCount = order.Items.Count();

            return orderModel;
        }

        public void FinishOrder(string userId, DateTime dateOfOrder)
        {
            var order = FindOrderByUserId(userId);

            order.DateOfOrder = dateOfOrder;

            order.IsFinished = true;

            this.data.SaveChanges();
        }

        private void AddItem(Item item)
        {
            this.data.Items.Add(item);

            this.data.SaveChanges();
        }

        private void AddOrder(Order order)
        {
            this.data.Orders.Add(order);

            this.data.SaveChanges();
        }
    }
}
