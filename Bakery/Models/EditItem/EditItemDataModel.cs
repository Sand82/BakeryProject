using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.EditItem
{
    public class EditItemDataModel
    {
        public int ItemId { get; set; }

        [Range(ItemMinValue, ItemMaxValue,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Quantity { get; set; }
    }
}
