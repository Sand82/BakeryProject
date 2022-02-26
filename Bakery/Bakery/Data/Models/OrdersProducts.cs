namespace Bakery.Data.Models
{
    public class OrdersProducts
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int BakedDishId { get; set; }
        public Product BakedDish { get; set; }
    }
}
