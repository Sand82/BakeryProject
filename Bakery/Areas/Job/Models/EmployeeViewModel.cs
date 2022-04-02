using Bakery.Data.Models;

namespace Bakery.Areas.Job.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FullName { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }
        
        public string Description { get; set; }
        
        public string Experience { get; set; }

        public IFormFile Autobiography { get; set; }

        public byte[] Image { get; set; }

        public string ApplayDate { get; set; } 

        public bool IsApproved { get; set; }        
    }
}
