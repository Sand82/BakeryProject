using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Ingredients = new HashSet<Ingredient>();            

            this.Votes = new HashSet<Vote>();

            this.Items = new HashSet<Item>();
        }

        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(ProductMaxLenght)]
        public string Name { get; set; }

        [Range(typeof(decimal), DecimalMinValue, DecimalMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        [StringLength(ImageMaxLenght)]

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public bool IsDelete { get; set; } = false;

        public ICollection<Ingredient> Ingredients { get; set; }        

        public ICollection<Vote>? Votes { get; set; }

        public ICollection<Item>? Items { get; set; }
        
    }
}
