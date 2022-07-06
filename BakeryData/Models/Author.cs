using System.ComponentModel.DataAnnotations;

using static BakeryData.Constants;

namespace BakeryData.Models
{
    public class Author
    {
        public Author()
        {
            this.Products = new HashSet<Product>();

            this.Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        
        [Required]
        [StringLength(MaxName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(MaxName)]
        public string LastName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }  
        
        public string? AuthorId { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
