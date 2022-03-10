using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Ingredients = new HashSet<Ingredient>();

            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(ProductMaxLenght)]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        [StringLength(ImageMaxLenght)]
        public string ImageUrl { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
