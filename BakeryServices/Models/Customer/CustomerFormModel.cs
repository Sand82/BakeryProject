using BakeryServices.Models.Orders;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace BakeryServices.Models.Customer
{
    public class CustomerFormModel
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
        [EmailAddress]
        [Display(Name = "Email address")]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        [StringLength(PhoneLength, MinimumLength = PhoneLength,
            ErrorMessage = "The field {0} is not valid! Must be exact 13 symbols.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(AdressMaxValue, MinimumLength = AdressMinValue,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Address { get; set; }
        
        public string? UserId { get; set; }

        public int? OrderId { get; set; }        

        public CreateOrderModel Order { get; set; }
    }
}
