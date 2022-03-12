using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(NameMaxLenght)]
        public string FullName { get; set; }
    }
}
