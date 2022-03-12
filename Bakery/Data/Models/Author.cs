using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(maxAuthorName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maxAuthorName)]
        public string LastName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }  
        
        public string AuthorId { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
