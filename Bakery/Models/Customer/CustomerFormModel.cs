using Bakery.Models.Order;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Models.Customer
{
    public class CustomerFormModel
    {
        [Required]
        [StringLength(MaxName, MinimumLength = MinName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(MaxName, MinimumLength = MinName)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(PhoneLength, MinimumLength = PhoneLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(AdressMaxValue, MinimumLength = AdressMinValue)]
        public string Address { get; set; }

        [Required]
        public string UserId { get; set; }

        public int OrderId { get; set; }        

        public CreateOrderModel Order { get; set; }
    }
}
