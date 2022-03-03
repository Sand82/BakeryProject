using System.ComponentModel.DataAnnotations;

namespace Bakery.Models.Bakeries
{
    public class AllProductQueryModel
    {
        public IEnumerable<string> Names { get; set; }

        [Display(Name = "Search by product")]
        public string SearchTerm { get; set; }

        public BakiesSorting Sorting { get; set; }

        public IEnumerable<AllProductViewModel> Products { get; set; }
    }
}
