using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
