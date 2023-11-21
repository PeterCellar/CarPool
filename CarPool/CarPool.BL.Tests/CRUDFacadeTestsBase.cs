using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CarPool.DAL.Tests;
using CarPool.DAL.Factories;
using CarPool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using CarPool.DAL;

namespace CarPool.BL.Tests
{

    public class CRUDFacadeTestsBase : IAsyncLifetime
    {
        protected CRUDFacadeTestsBase()
        {
            // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
            // DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
            DbContextFactory = new SqlLiteDbContextFactory(GetType().FullName!, seedTestingData: true);

            UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new[]
                {
                        typeof(BusinessLogic),
                    });
                cfg.AddCollectionMappers();

                using var dbContext = DbContextFactory.CreateDbContext();
                cfg.UseEntityFrameworkCoreModel<CarPoolDbContext>(dbContext.Model);
            }
            );
            Mapper = new Mapper(configuration);
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        protected IDbContextFactory<CarPoolDbContext> DbContextFactory { get; }

        protected Mapper Mapper { get; }

        protected IUnitOfWorkFactory UnitOfWorkFactory { get; }

        public async Task InitializeAsync()
        {
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            await dbx.Database.EnsureDeletedAsync();
            await dbx.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            await dbx.Database.EnsureDeletedAsync();
        }
    }

}