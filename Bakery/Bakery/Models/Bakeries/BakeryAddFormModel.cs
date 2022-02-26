using Bakery.Data.Models;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Bakery
{
    public class BakeryAddFormModel
    {
        [Required]
        [StringLength(ProductMaxLenght)]
        public string Name { get; set; }

        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        [StringLength(ImageMaxLenght)]
        public string ImageUrl { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
