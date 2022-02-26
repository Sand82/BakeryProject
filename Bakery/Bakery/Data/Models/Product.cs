using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Ingredients = new HashSet<Ingredient>();

            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public int ProductId { get; set; }

        public Ingredient Ingredient { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
