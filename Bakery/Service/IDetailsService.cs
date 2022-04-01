using Bakery.Models.Details;

namespace Bakery.Service
{
    public interface IDetailsService
    {
       void AddProductToOrder(int productId,int orderId,int quantity);
    }
}
