
using CarPool.DAL.Entities;
using CarPool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace CarPool.DAL.Tests
{
    public class RideTests : DbContextTestsBase
    {
        public RideTests() : base()
        {

        }


        [Fact]
        public async Task Check_Seeded_Ride1()
        {
            var entities = await CarPoolDbContextSUT.Rides.ToArrayAsync();
            Assert.Contains(entities, item => item.Id == RideSeed.Ride1.Id);
        }

        [Fact]
        public async Task GetById_RideDriver()
        {
            var entity = await CarPoolDbContextSUT.Rides.SingleAsync(i => i.Id == RideSeed.Ride1.Id);
            DeepAssert.Equal(RideSeed.Ride1, entity);
        }

       
        [Fact]
        public async Task AddRide()
        {
            RideEntity entity = new(
                Id: Guid.Parse("40363CA3-7EC2-4CD8-8251-E5924A5B6D59"),
                StartTime: new DateTime(2002, 5, 5),
                EndTime: new DateTime(2002, 5, 5),
                StartLocation: "Misto C",
                EndLocation: "Misto D",
                UsedCarId: Guid.Parse(input: "3A8490B2-3BD3-410D-A513-6340699E9FA5"),
                DriverId: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75")
                );

            CarPoolDbContextSUT.Rides.Add(entity);
            await CarPoolDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Rides.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntities);
        }
        [Fact]
        public async Task DeleteRide()
        {
            CarPoolDbContextSUT.Remove(
                CarPoolDbContextSUT.Rides.Single(i => i.Id == RideSeed.Ride1.Id));
            await CarPoolDbContextSUT.SaveChangesAsync();

            var entities = await CarPoolDbContextSUT.Rides.ToArrayAsync();

            Assert.False(await CarPoolDbContextSUT.Rides.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }
    }
}
