using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarPool.DAL.Factories
{
    /// <summary>
    /// EF Core CLI migration generation uses this DbContext to create model and migration
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarPoolDbContext>
    {
        public CarPoolDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<CarPoolDbContext> builder = new();
            builder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog = CarPool;
                MultipleActiveResultSets = True;
                Integrated Security = True; ");

            return new CarPoolDbContext(builder.Options);
        }
    }
}
