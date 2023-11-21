using System;
using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.DAL.Seeds;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarPool.BL.Tests
{
    public sealed class RideFacadeTests : CRUDFacadeTestsBase
    {

        private readonly RideFacade _rideFacadeSUT;

        public RideFacadeTests() : base()
        {
            _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task GetAll_Single_SeededRide()
        {
            var rides = await _rideFacadeSUT.GetAsync();
            var retrieved = rides.Single(i => i.Id == RideSeed.Ride1.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeed.Ride1), retrieved);
        }

        [Fact]
        public async Task GetallSingle_SeededRide1()
        {
            var ridesFromFacade = await _rideFacadeSUT.GetAsync();
            var retrieved = ridesFromFacade.Single(i => i.Id == RideSeed.Ride1.Id);

            DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeed.Ride1), retrieved);
        }

        [Fact]
        public async Task GetSeededRide1_byTime()
        {

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Rides
                .Include(b => b.Driver)
                .Include(c => c.UsedCar)
                .Include(e => e.Passengers)
                .ToArrayAsync();
            var rideFromDb = cars.Single(i => i.Id == RideSeed.Ride1.Id);
            var rideModel = Mapper.Map<RideListModel>(rideFromDb);

            var returnedModel = await _rideFacadeSUT.GetByTime(new DateTime(2000, 5, 5, 16, 0, 0));

            Assert.Contains(rideModel, returnedModel);
        }

        [Fact]
        public async Task GetSeededRide1_byPlace()
        {

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Rides
                .Include(b => b.Driver)
                .Include(c => c.UsedCar)
                .Include(e => e.Passengers)
                .ToArrayAsync();
            var rideFromDb = cars.Single(i => i.Id == RideSeed.Ride1.Id);
            var rideModel = Mapper.Map<RideListModel>(rideFromDb);

            var returnedModel = await _rideFacadeSUT.GetByPlace(RideSeed.Ride1.StartLocation, RideSeed.Ride1.EndLocation);

            Assert.Contains(rideModel, returnedModel);
        }

        [Fact]
        public async Task Create_rideWithoutCar_Throws()
        {

            var ride = new RideDetailModel(

                     StartLocation: "misto A",
                     EndLocation: "misto B",
                     StartTime: new DateTime(2000, 5, 20),
                     EndTime: new DateTime(2000, 5, 20))
            {
                Driver = new UserListModel(
                     UserName: "Mickey",
                     Name: "Petr",
                     Surname: "Novák"),
                Passengers = {
                    new NumberOfRidesDetailModel()
                    {
                        User = new UserDetailModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                        Ride = new RideDetailModel(StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
                    }
                }
            };

            await Assert.ThrowsAsync<DbUpdateException>(() => _rideFacadeSUT.SaveAsync(ride));
        }

        [Fact]
        public async Task Create_rideWithoutDriver_Throws()
        {
            var ride = new RideDetailModel(

                    StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
            {
                UsedCar = new CarListModel(
                Manufacturer: "VW",
                Type: "Golf",
                FirstRegistration: new DateTime(2002, 5, 4),
                Seats: 5)
                {
                    Owner = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                },
                Passengers = {
                    new NumberOfRidesDetailModel()
                    {
                        User = new UserDetailModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                        Ride = new RideDetailModel(StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
                    }
                }
            };

            await Assert.ThrowsAsync<DbUpdateException>(() => _rideFacadeSUT.SaveAsync(ride));
        }

        [Fact]
        public async Task Add_Ride_NoPassengers()
        {
            //Arange
            var ride = new RideDetailModel(
                    
                    StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
            {

                Driver = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                UsedCar = new CarListModel(
                Manufacturer: "VW",
                Type: "Golf",
                FirstRegistration: new DateTime(2002, 5, 4),
                Seats: 5
            )
                {
                    Owner = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák")
                },
               
        };
            
            //Act
            ride = await _rideFacadeSUT.SaveAsync(ride);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var rides = await dbxAssert.Rides
                .Include(b => b.Driver)
                .Include(c => c.UsedCar)
                .Include(d => d.UsedCar.Owner)
                .ToArrayAsync();
            var rideFromDb = rides.Single(i => i.Id == ride.Id);
            var mapped = Mapper.Map<RideDetailModel>(rideFromDb);

            DeepAssert.Equal(ride, mapped);
        }

       /*
       * In the next tests the seeded is pulled out of the db
       * so that we can include navigation properties,
       * then compare with mapped model from Facade
       */

        [Fact]
        public async Task NumberOfRides_test()
        {
            //Arange
            var ride = new RideDetailModel(

                    StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
            {

                Driver = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                UsedCar = new CarListModel(
                Manufacturer: "VW",
                Type: "Golf",
                FirstRegistration: new DateTime(2002, 5, 4),
                Seats: 5)
                {
                    Owner = new UserListModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák")
                },
                Passengers = {
                    new NumberOfRidesDetailModel()
                    {
                        User = new UserDetailModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák"),

                        Ride = new RideDetailModel(StartLocation: "misto A",
                    EndLocation: "misto B",
                    StartTime: new DateTime(2000, 5, 20),
                    EndTime: new DateTime(2000, 5, 20))
                    }
                }
            };

            //Act
            ride = await _rideFacadeSUT.SaveAsync(ride);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var rides = await dbxAssert.Rides
                .Include(b => b.Driver)
                .Include(c => c.UsedCar)
                .Include(e => e.Passengers)
                .ToArrayAsync();
            var rideFromDb = rides.Single(i => i.Id == ride.Id);
            var mapped = Mapper.Map<RideDetailModel>(ride);

            DeepAssert.Equal(ride, mapped);
        }
   
        [Fact]
        public async Task SeededRide_DeletedBy_Id()
        {
            await _rideFacadeSUT.DeleteAsync(RideSeed.Ride1.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }

        [Fact]
        public async Task Getall_checkContainsRide()
        {
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var cars = await dbxAssert.Rides
                .Include(b => b.Driver)
                .Include(c => c.UsedCar)
                .Include(e => e.Passengers)
                .ToArrayAsync();
            var rideFromDb = cars.Single(i => i.Id == RideSeed.Ride1.Id);
            var rideModel = Mapper.Map<RideListModel>(rideFromDb);

            var returnedModel = await _rideFacadeSUT.GetAsync();

            Assert.Contains(rideModel, returnedModel);
        }
    }
}
