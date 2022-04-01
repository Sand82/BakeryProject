using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class OrdersProducts
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        [Range(1, 200)]
        public int ProductQuantity { get; set; }
    }
}
