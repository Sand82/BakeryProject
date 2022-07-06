namespace BakeryServices.Models.Employees
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FullName { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }        
       
        public string Experience { get; set; }       

        public string ApplayDate { get; set; } 

        public bool IsApproved { get; set; }        
    }
}
