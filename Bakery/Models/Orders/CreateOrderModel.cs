using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Orders
{
    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            items = new List<ItemFormViewModel>();
        }

        public int Id { get; set; }

        public bool? IsFinished { get; set; }

        public string? TotallPrice { get; set; }

        public string? DateOfOrder { get; set; }

        [Required]
        [Display(Name = "Preparation day")]
        [StringLength(DateLength, MinimumLength = DateLength)]
        public string DateOfDelivery { get; set; }

        public int? ItemsCount { get; set; }

        public IList<ItemFormViewModel>? items { get; set; }

    }
}
