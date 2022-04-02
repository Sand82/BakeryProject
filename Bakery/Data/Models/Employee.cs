using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxName)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [Range(AgeMinValue, AgeMaxValue)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        [StringLength(EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(PhoneLength)]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght)]
        public string Description { get; set; }

        [Required]
        [StringLength(ExperienceMaxLength)]
        public string Experience { get; set; }

        public byte[] Autobiography { get; set; }

        public byte[]? Image { get; set; }

        public DateTime ApplayDate { get; set; } = DateTime.UtcNow;

        public bool? IsApproved { get; set; }        

        public int? AuthorId { get; set; }

        public Author? Author { get; set; }
    }
}
