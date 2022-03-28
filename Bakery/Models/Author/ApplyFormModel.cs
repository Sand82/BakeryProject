using System.ComponentModel.DataAnnotations;
using static Bakery.Data.Constants;

namespace Bakery.Models.Author
{
    public class ApplyFormModel
    {
        [Required]
        [StringLength(MaxName, MinimumLength = MinName,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(PhoneLength,
            ErrorMessage = "The field {0} is not valid! Must be exact {1} symbols.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Description { get; set; }

        public int? Experience { get; set; }

        public bool IsApproved { get; set; }
    }
}
