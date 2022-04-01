using Bakery.Data.Models;

namespace Bakery.Service
{
    public interface IDetailsService
    {
        OrdersProducts AddProductToOrder(int productId,int orderId,int quantity);
    }
}
