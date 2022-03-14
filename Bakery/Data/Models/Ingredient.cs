using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Products = new HashSet<ProductsIngredients>();
        }

        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public ICollection<ProductsIngredients> Products { get; set; }
    }
}
