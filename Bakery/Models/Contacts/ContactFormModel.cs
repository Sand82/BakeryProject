using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Contacts
{
    public class ContactFormModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(MaxName, MinimumLength = MinName,
           ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(MaxName, MinimumLength = MinName,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string LastName { get; set; }

        [Required]       
        [StringLength(SubjectMaxValue, MinimumLength = SubjectMinValue,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Email address")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        [StringLength(PhoneLength, MinimumLength = PhoneLength,
            ErrorMessage = "The field {0} is not valid! Must be exact 13 symbols.")]
        public string Phone { get; set; }

        [Required]
        [MinLength(MassageMinValue)]
        public string Massage { get; set; }

        public int CustomerId { get; set; }

    }
}
