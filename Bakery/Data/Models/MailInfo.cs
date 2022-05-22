using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class MailInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxName)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(MaxName)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(EmailMaxLength)]
        public string? Email { get; set; }

        [Required]
        [StringLength(PhoneLength)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(SubjectMaxValue)]
        public string? Subject { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string? Massage { get; set; }
        
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
