using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Factories
{
    public class SqlLiteDbContextFactory : IDbContextFactory<CarPoolDbContext>
    {
        private readonly string _databaseName;
        private readonly bool _seedTestingData;

        public SqlLiteDbContextFactory(string databaseName, bool seedTestingData = false)
        {
            _databaseName = databaseName;
            _seedTestingData = seedTestingData;
        }
        public CarPoolDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<CarPoolDbContext> builder = new();
            builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

            // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            // builder.EnableSensitiveDataLogging();

            return new CarPoolDbContext(builder.Options, _seedTestingData);
        }
    }
}
