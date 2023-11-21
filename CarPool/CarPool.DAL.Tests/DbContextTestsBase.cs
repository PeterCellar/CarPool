using CarPool.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace CarPool.DAL.Tests;
public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase()
    {

        // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
        //DbContextFactory = new SqlServerDbContextTestingFactory(GetType().FullName!);
        DbContextFactory = new SqlLiteDbContextFactory(GetType().FullName!, seedTestingData: true);

        CarPoolDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<CarPoolDbContext> DbContextFactory { get; }
    protected CarPoolDbContext CarPoolDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await CarPoolDbContextSUT.Database.EnsureDeletedAsync();
        await CarPoolDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await CarPoolDbContextSUT.Database.EnsureDeletedAsync();
        await CarPoolDbContextSUT.DisposeAsync();
    }
}