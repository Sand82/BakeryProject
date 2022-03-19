using Bakery.Models.Bakeries;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models.Items
{
    public class DetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public int VoteCount { get; set; }

        [Range(1,5)]
        public int Vote { get; set; }

        public ICollection<IngredientAddFormModel> Ingridients { get; set; }
    }
}
