using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data
{
    public class BackeryDbContext : IdentityDbContext
    {
        public BackeryDbContext(DbContextOptions<BackeryDbContext> options)
            : base(options)
        {
        }
    }
}