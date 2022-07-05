using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Bakery
{
    public class BakeryFormModel
    {
        [Required]
        [StringLength(ProductMaxLenght, MinimumLength =ProductMinLenght,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Name { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength =DescriptionMinLenght,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Description { get; set; }
        
        [Url]
        [Required]        
        [Display(Name = "Image URL")]        
        public string ImageUrl { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<IngredientAddFormModel> Ingredients { get; set; }

        public IEnumerable<BakryCategoryViewModel>? Categories { get; set; } 

        //public ICollection<Order> Orders { get; set; }
    }
}
