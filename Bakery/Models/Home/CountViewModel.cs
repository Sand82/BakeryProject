using Bakery.Models.Bakeries;

namespace Bakery.Models.Home
{
    public class CountViewModel
    {

        public int ProductCount { get; set; }

        public int IngredientCount { get; set; }

        public IEnumerable<AllProductViewModel> AllProductViewModel { get; set; }
    }
}
