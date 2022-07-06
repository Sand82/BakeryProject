using Bakery.Data;
using Bakery.Data.Models;
using BakeryServices.Models.Customer;
using BakeryServices.Models.Items;
using BakeryServices.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BakeryServices.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly BakeryDbContext data;

        public OrderService(BakeryDbContext data)
        {
            this.data = data;
        }

        public async Task<Order> CreatOrder(string userId)
        {
            var order = new Order
            {
                UserId = userId,
            };

            await AddOrder(order);

            return order;
        }

        public async Task<Item> CreateItem(int id, string name, decimal price, int quantity, string userId)
        {
            var item = new Item
            {
                ProductId = id,
                ProductName = name,
                ProductPrice = price,
                Quantity = quantity,
            };

            await AddItem(item);

            return item;
        }

        public async Task AddItemInOrder(Item item, Order order)
        {
            order.Items.Add(item);

            await this.data.SaveChangesAsync();
        }

        public async Task<Order> FindOrderByUserId(string userId)
        {
            var order = await this.data
            .Orders
            .Include(i => i.Items)
            .Where(o => o.UserId == userId && o.IsFinished == false)
            .FirstOrDefaultAsync();

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

        public async Task<Order> FinishOrder(Order order, DateTime dateOfDelivery)
        {
            order.DateOfDelivery = dateOfDelivery;

            order.IsFinished = true;

            await this.data.SaveChangesAsync();

            return order;
        }

        public async Task<CustomerFormModel> GetCustomer(string userId)
        {
            var customer = await this.data.Customers
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
            .FirstOrDefaultAsync();

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

        private async Task AddItem(Item item)
        {
            await this.data.Items.AddAsync(item);

            await this.data.SaveChangesAsync();
        }

        private async Task AddOrder(Order order)
        {
            await this.data.Orders.AddAsync(order);

            await this.data.SaveChangesAsync();
        }
    }
}
