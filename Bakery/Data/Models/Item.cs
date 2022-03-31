using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Item
    {
        public Item()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int Id { get; set; }                    

        [Range(ItemMinValue, ItemMaxValue)]
        public int Quantity { get; set; }

        public int? ProductId { get; set; }

        public ICollection<Order> Orders { get; set; }
        
    }
}
