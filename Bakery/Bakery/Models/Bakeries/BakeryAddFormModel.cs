using Bakery.Data.Models;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Bakery
{
    public class BakeryAddFormModel
    {
        [Required]
        [StringLength(ProductMaxLenght,
            MinimumLength =ProductMinLenght,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Name { get; set; }

        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght,
            MinimumLength =DescriptionMinLenght,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Description { get; set; }
        
        [Url]
        [Required]        
        [Display(Name = "Image URL")]        
        public string ImageUrl { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
