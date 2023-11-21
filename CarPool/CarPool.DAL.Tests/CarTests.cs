using CarPool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace CarPool.DAL.Tests
{
    public class CarTests : DbContextTestsBase
    {
        public CarTests() : base()
        {

        }


        [Fact]
        public async Task Check_Seeded_Fiat()
        {
            var entities = await CarPoolDbContextSUT.Cars.ToArrayAsync();
            Assert.Contains(entities, item => item.Id == CarSeed.Fiat.Id);
        }

        [Fact]
        public async Task GetById_Car_Renault_Retrieved()
        {
            var entity = await CarPoolDbContextSUT.Cars.SingleAsync(i => i.Id == CarSeed.Renault.Id);
            DeepAssert.Equal(CarSeed.Renault, entity);
        }

        [Fact]
        public async Task DeleteRenault_hasNoRides()
        {
            CarPoolDbContextSUT.Remove(
                CarPoolDbContextSUT.Cars.Single(i => i.Id == CarSeed.Renault.Id));
            await CarPoolDbContextSUT.SaveChangesAsync();

            var entities = await CarPoolDbContextSUT.Cars.ToArrayAsync();


            Assert.False(await CarPoolDbContextSUT.Cars.AnyAsync(i => i.Id == CarSeed.Renault.Id));
        }

        [Fact]
        public async Task DeleteFiat_Ride_deleted()
        {

            CarPoolDbContextSUT.Remove(
                CarPoolDbContextSUT.Cars.Single(i => i.Id == CarSeed.Fiat.Id));
            await CarPoolDbContextSUT.SaveChangesAsync();

            var entities = await CarPoolDbContextSUT.Cars.ToArrayAsync();

            Assert.False(await CarPoolDbContextSUT.Cars.AnyAsync(i => i.Id == CarSeed.Fiat.Id));
            Assert.False(await CarPoolDbContextSUT.Rides.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }

    }
}
