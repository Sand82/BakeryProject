using Bakery.Models.Author;

namespace Bakery.Models.Bakeries
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }

        public IList<EmployeeDetailsViewModel> Employees { get; set; }
    }
}
