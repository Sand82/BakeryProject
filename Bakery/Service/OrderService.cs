using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models;
using Bakery.Models.Customer;
using Bakery.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Bakery.Service
{
    public class OrderService : IOrderService
    {
        private readonly BakeryDbContext data;

        public OrderService(BakeryDbContext data)
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
                ProductId = id,
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
                order.Items.Add(item);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();            
        }

        public Order FindOrderByUserId(string userId)
        {
            var order = new Order();

            Task.Run(() =>
            {
                  order = this.data
                 .Orders
                 .Include(i => i.Items)
                 .Where(o => o.UserId == userId && o.IsFinished == false)
                 .FirstOrDefault();

            }).GetAwaiter().GetResult();            

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

        public Order FinishOrder(Order order, DateTime dateOfDelivery)
        {
            order.DateOfDelivery = dateOfDelivery;

            order.IsFinished = true;

            this.data.SaveChanges();

            return order;
        }

        public CustomerFormModel GetCustomer(string userId)
        {
            CustomerFormModel? customer = null;

            Task.Run(() => 
            {
                customer = this.data.Customers
                .OrderByDescending(x => x.Id)
                .Where(c => c.UserId == userId)
                .Select(c => new CustomerFormModel 
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Address = c.Adress,
                    UserId = userId,
                    PhoneNumber = c.PhoneNumber,
                })
                .FirstOrDefault();

            }).GetAwaiter().GetResult();

            return customer;
        }

        public (bool, DateTime) TryParceDate(string date)
        {
            DateTime dateOfOrder;

            var isValidDate = DateTime.TryParseExact(
                date.ToString(),
                "dd.MM.yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateOfOrder);

            return (isValidDate, dateOfOrder);
        }

        private void AddItem(Item item)
        {
            Task.Run(() =>
            {
                this.data.Items.Add(item);

                this.data.SaveChanges();

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
