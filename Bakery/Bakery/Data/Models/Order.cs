namespace Bakery.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        public string CustomerName { get; set; }

        public DateTime DateOfOrder { get; set; }

        public DateTime DateOfDelivery { get; set; }

        public int OrderProductsCount { get; set; }

        public decimal TottalPrice { get; set; }

        public int BakedDishId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
