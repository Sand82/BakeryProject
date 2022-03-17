using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static Bakery.Data.Constants;

namespace Bakery.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(MaxName)]
        public string FullName { get; set; }
    }
}
