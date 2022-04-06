using Bakery.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bakery.Tests.Mock
{
    public static class DatabaseMock
    {
        public static BakeryDbContext Instance
        {
            get 
            {
                var dbContextOptions = new DbContextOptionsBuilder<BakeryDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

                return new BakeryDbContext(dbContextOptions);
            }
        }
    }
}
