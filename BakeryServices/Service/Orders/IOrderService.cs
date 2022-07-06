using BakeryData.Models;
using BakeryServices.Models.Customer;
using BakeryServices.Models.Orders;

namespace BakeryServices.Service.Orders
{
    public interface IOrderService
    {
        Task<Order> CreatOrder(string userId);

        Task<Item> CreateItem(int id, string name, decimal price, int quantity, string userId);

        Task AddItemInOrder(Item item, Order order);

        Task<Order> FindOrderByUserId(string userId);

        CreateOrderModel CreateOrderModel(Order order);

        Task<Order> FinishOrder(Order order, DateTime dateOfOrder);

        (bool, DateTime) TryParceDate(string date);

        Task<CustomerFormModel> GetCustomer(string userId);
    }
}
