using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Details;

namespace Bakery.Service
{
    public class DetailsService : IDetailsService
    {

        private readonly BackeryDbContext data;

        public DetailsService(BackeryDbContext data)
        {
            this.data = data;
        }

        public OrdersProducts AddProductToOrder(int productId, int orderId, int quantity)
        {
            var orderProduct = new OrdersProducts
            {
                OrderId = orderId,
                ProductId = productId,
                ProductQuantity = quantity
            };

            Task.Run(() =>
            {
                this.data.OrdersProducts.Add(orderProduct);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();

            return orderProduct;
        }       
    }
}
