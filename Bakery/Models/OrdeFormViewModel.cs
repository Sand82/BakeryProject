using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
    public class OrdeFormViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }
        
        public int Quantity { get; set; }
    }
}
