using Microsoft.EntityFrameworkCore;
using MinimalApiDotNet.Models;

namespace MinimalApiDotNet.Data
{
    public class CarDb: DbContext
    {
        public CarDb(DbContextOptions<CarDb> options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; }
    }
}
