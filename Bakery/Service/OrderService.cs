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
            var order = new Order();

            Task.Run(() =>
            {
                order = new Order
                {
                    UserId = userId,
                };

                AddOrder(order);

            }).GetAwaiter().GetResult();

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

            Task.Run(() =>
            {               
                AddItem(item);

            }).GetAwaiter().GetResult();

            return item;
        }

        public void AddItemInOrder(Item item, Order order)
        {
            Task.Run(() =>
            {
                //order.Items.Add(item);

                //this.data.SaveChanges();

            }).GetAwaiter().GetResult();            
        }

        public Order FindOrderByUserId(string userId)
        {
            var order = new Order();

            Task.Run(() =>
            {
                order = this.data
               .Orders    
               .Include(o => o.Products)               
               .Where(o => o.UserId == userId && o.IsFinished == false)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();            

            return order;
        }

        public int FindOrderIdByUserId(string userId)
        {
            var orderId = 0;

            Task.Run(() =>
            {
                orderId = this.data
               .Orders
               .Where(o => o.UserId == userId && o.IsFinished == false)
               .Select(x => x.Id)
               .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return orderId;
        }

        public CreateOrderModel CreateOrderModel(string userId)
        {
            var orderProducts = new OrdersProducts();

            Task.Run(() =>
            {
                orderProducts = this.data.OrdersProducts
                .Include(o=> o.Order).ThenInclude(p => p.Products)
                .Where(o => o.Order.UserId == userId && o.Order.IsFinished == false)                               
                .FirstOrDefault();

            }).GetAwaiter().GetResult();

            var orderModel = new CreateOrderModel
            {
                Id = orderProducts.Order.Id,
                IsFinished = orderProducts.Order.IsFinished,
                DateOfOrder = orderProducts.Order.DateOfOrder.ToString("dd.mm.yyyy")
            };

            var totalPrice = 0.0m;

            foreach (var product in orderProducts.Order.Products)
            {
                totalPrice += product.Price * orderProducts.ProductQuantity;

                var newItem = new ItemFormViewModel
                {
                    Name = product.Name,
                    Price = product.Price.ToString("f2"),
                    Quantity = orderProducts.ProductQuantity
                };

                orderModel.items.Add(newItem);
            }

            orderModel.TotallPrice = totalPrice.ToString("f2");

            orderModel.ItemsCount = orderProducts.Order.Products.Count();

            return orderModel;
        }

        public Order FinishOrder(Order order, DateTime dateOfDelivery)
        {
            order.DateOfDelivery = dateOfDelivery;

            order.IsFinished = true;

            this.data.SaveChanges();

            return order;
        }

        private void AddItem(Item item)
        {
            Task.Run(() =>
            {
                //this.data.Items.Add(item);

                //this.data.SaveChanges();

            }).GetAwaiter().GetResult();           
        }

        private void AddOrder(Order order)
        {
            Task.Run(() =>
            {
                this.data.Orders.Add(order);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();           
        }       
    }
}
