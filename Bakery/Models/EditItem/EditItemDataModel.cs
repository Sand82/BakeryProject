using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.EditItem
{
    public class EditItemDataModel
    {
        public int ItemId { get; set; }

        [Range(ItemMinValue, ItemMaxValue)]
        public int Quantity { get; set; }
    }
}
