using System.ComponentModel.DataAnnotations;

namespace Bakery.Data.Models
{
    public class Order
    {
        public Order()
        {
            this.Items = new HashSet<Item>();  
        }

        [Key]
        public int Id { get; init; }

        public string UserId { get; set; }

        public DateTime DateOfOrder { get; set; } = DateTime.UtcNow;

        public DateTime DateOfDelivery { get; set; }               

        public bool IsFinished { get; set; } = false;       

        public ICollection<Item> Items { get; set; }
    }
}
