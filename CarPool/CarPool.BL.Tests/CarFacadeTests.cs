using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.DAL.Seeds;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarPool.BL.Tests
{
    public sealed class CarFacadeTests : CRUDFacadeTestsBase
    {
        private readonly CarFacade _carFacadeSUT;

        public CarFacadeTests() : base()
        {
            _carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_carWithUser_DoesNotThrow()
        {

            var model = new CarDetailModel
            (
                Manufacturer: @"Manufacturer",
                Type: @"Type",
                FirstRegistration: new System.DateTime(2004, 2, 2),
                Seats: 4

            )
            {
                Owner = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák")
        };

           await _carFacadeSUT.SaveAsync(model);
        }

        [Fact]
        public async Task Create_carWithoutOwner_Throws()
        {

            var model = new CarDetailModel
            (
                Manufacturer: @"Manufacturer",
                Type: @"Type",
                FirstRegistration: new System.DateTime(2004, 2, 2),
                Seats: 4

            );
            await Assert.ThrowsAsync<DbUpdateException>(() => _carFacadeSUT.SaveAsync(model));
        }

        [Fact]
        public async Task SeededFiat_DeleteById_Deleted_cascade()
        {
            await _carFacadeSUT.DeleteAsync(CarSeed.Fiat.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeed.Fiat.Id));
            Assert.False(await dbxAssert.Rides.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }

        /*
        * In the next tests the seeded is pulled out of the db
        * so that we can include navigation properties,
        * then compare with mapped model from Facade
        */

        [Fact]
        public async Task GetAll_Single_SeededFiat()
        {
            

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Cars.Include(b => b.Owner).ToArrayAsync();
            var carFromDb = cars.Single(i => i.Id == CarSeed.Fiat.Id);
            var carMOdel = Mapper.Map<CarDetailModel>(carFromDb);

            var cars_from_facade = await _carFacadeSUT.GetAsync();
            var retrieved = cars.Single(i => i.Id == CarSeed.Fiat.Id);

            DeepAssert.Equals(carMOdel, retrieved);

        }

        [Fact]
        public async Task GetById_SeededFiat()
        {

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Cars.Include(b => b.Owner).ToArrayAsync();
            var carFromDb = cars.Single(i => i.Id == CarSeed.Fiat.Id);
            var carMOdel = Mapper.Map<CarDetailModel>(carFromDb);

            var retrieved = await _carFacadeSUT.GetAsync(CarSeed.Fiat.Id);

            DeepAssert.Equals(carMOdel, retrieved);
        }

        [Fact]
        public async Task Getall_checkContainsFiat()
        {

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Cars.Include(b => b.Owner).ToArrayAsync();
            var carFromDb = cars.Single(i => i.Id == CarSeed.Fiat.Id);
            var carMOdel = Mapper.Map<CarListModel>(carFromDb);

            var returnedModel = await _carFacadeSUT.GetAsync();

            Assert.Contains(carMOdel, returnedModel);
        }
        
        [Fact]
        public async Task NewCar_Insert_CarAdded()
        {

            var car = new CarDetailModel(
                Manufacturer: "VW",
                Type: "Golf",
                FirstRegistration: new System.DateTime(2002, 5, 4),
                Seats: 5
            )
            {
                Owner = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák")
            };

            car = await _carFacadeSUT.SaveAsync(car);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Cars.Include(b => b.Owner).ToArrayAsync();
            var carFromDb = cars.Single(i => i.Id == car.Id);
            var carMOdel = Mapper.Map<CarDetailModel>(carFromDb);

            DeepAssert.Equal(car, carMOdel);
        }

        [Fact]
        public async Task Delete_Renault_Ride1_NotAffected()
        {
            await _carFacadeSUT.DeleteAsync(CarSeed.Renault.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeed.Renault.Id));
            var rides = await dbxAssert.Rides
                .ToArrayAsync();
            var rideFromDb = rides.Single(i => i.Id == RideSeed.Ride1.Id);

            Assert.Contains(rides, item => item.Id == RideSeed.Ride1.Id);
        }      
    }
}
