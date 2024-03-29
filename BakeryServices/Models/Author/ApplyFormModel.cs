﻿using System.ComponentModel.DataAnnotations;
using static BakeryData.Constants;

namespace BakeryServices.Models.Author
{
    public class ApplyFormModel
    {
        [Required]
        [StringLength(MaxName, MinimumLength = MinName,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string FullName { get; set; }

        [Required]
        [Range(AgeMinValue, AgeMaxValue)]
        public int Age { get; set; }

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

        [Required]
        [StringLength(ExperienceMaxLength, MinimumLength = ExperienceMinLength,
            ErrorMessage = "The field {0} is not valid! Must be between of {2} and {1} symbols.")]
        public string Experience { get; set; }       

        public bool IsApproved { get; set; }

        public string FileId { get; set; }

        public string ImageId { get; set; }
    }
}
