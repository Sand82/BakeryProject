﻿using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class Customer
    {       

        [Key]
        public int Id { get; set; }        

        [Required]
        [MaxLength(MaxName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(MaxName)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(PhoneLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(AdressMaxValue)]
        public string Adress { get; set; }       
       
        [Required]
        public string UserId { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
