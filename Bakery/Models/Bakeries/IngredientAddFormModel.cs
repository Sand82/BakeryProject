using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Bakeries
{
    public class IngredientAddFormModel
    {
        [Required]
        [StringLength(IngredientMaxName, MinimumLength = IngredientMinName )]
        public string Name { get; set; }
    }
}
