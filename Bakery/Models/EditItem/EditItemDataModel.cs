using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.EditItem
{
    public class EditItemDataModel
    {
        public int ItemId { get; set; }

        [Range(ItemMaxValue, ItemMinValue)]
        public int Quantity { get; set; }
    }
}
