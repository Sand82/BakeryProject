using System.ComponentModel.DataAnnotations;

namespace Bakery.Models.Bakeries
{
    public class AllProductQueryModel
    {
        public const int ProductPerPage = 4;

        public int CurrentPage { get; set; } = 1;

        public int TotalProduct { get; set; }

        [Display(Name = "Search by product")]
        public string SearchTerm { get; set; }

        [Display(Name = "Sorting by: ")]
        public BakiesSorting Sorting { get; set; }

        public IEnumerable<AllProductViewModel> Products { get; set; }
    }
}
