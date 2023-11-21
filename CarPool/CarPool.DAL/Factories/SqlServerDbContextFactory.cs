using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Factories
{
    public class SqlServerDbContextFactory : IDbContextFactory<CarPoolDbContext>
    {
        private readonly string _connectionString;
        private readonly bool _seedDemoData;

        public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
        {
            _connectionString = connectionString;
            _seedDemoData = seedDemoData;
        }

        public CarPoolDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarPoolDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            //optionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            //optionsBuilder.EnableSensitiveDataLogging();

            return new CarPoolDbContext(optionsBuilder.Options, _seedDemoData);
        }
    }
}
