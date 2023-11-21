using CarPool.DAL.Entities;
using CarPool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
namespace CarPool.DAL.Tests
{
    public class UserTests : DbContextTestsBase
    {
        public UserTests() : base()
        {

        }


        [Fact]
        public async Task Check_Seeded_Martin()
        {
            var entities = await CarPoolDbContextSUT.Users.ToArrayAsync();
            Assert.Contains(entities, item => item.Id == UserSeed.Martin.Id);
        }

        [Fact]
        public async Task GetById_User_MartinRetrieved()
        {
            var entity = await CarPoolDbContextSUT.Users.SingleAsync(i => i.Id == UserSeed.Martin.Id);
            DeepAssert.Equal(UserSeed.Martin, entity);
        }

        [Fact]
        public async Task DeleteUser_cascadeDeletes_CarAndRide()
        {            
            CarPoolDbContextSUT.Users.Remove(UserSeed.Martin);
            await CarPoolDbContextSUT.SaveChangesAsync();

            //Checking if user and his car were both deleted
            var users = await CarPoolDbContextSUT.Users.ToArrayAsync();
            var cars = await CarPoolDbContextSUT.Cars.ToArrayAsync();
            Assert.False(await CarPoolDbContextSUT.Users.AnyAsync(i => i.Id == UserSeed.Martin.Id));
            Assert.False(await CarPoolDbContextSUT.Cars.AnyAsync(i => i.Id == CarSeed.Fiat.Id));
            Assert.False(await CarPoolDbContextSUT.Rides.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }
        [Fact]
        public async Task DeleteUser_ride_stays()
        {
            CarPoolDbContextSUT.Users.Remove(UserSeed.Jakub);
            await CarPoolDbContextSUT.SaveChangesAsync();
            
            var entities = await CarPoolDbContextSUT.Rides.ToArrayAsync();

            Assert.False(await CarPoolDbContextSUT.Users.AnyAsync(i => i.Id == UserSeed.Jakub.Id));
            Assert.Contains(entities, item => item.Id == RideSeed.Ride1.Id);
        }
        [Fact]
        public async Task Add_new_User()
        {
            UserEntity entity = new(
                Id: Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name: "Josef",
                Surname: "Novák",
                UserName: "Kohuz",
                ImageUrl: ""
                );

            CarPoolDbContextSUT.Users.Add(entity);
            await CarPoolDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntities);
        }

        [Fact]
        public async Task Add_new_User2()
        {
            UserEntity entity = new(
                Id: Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name: "Petr",
                Surname: "Malý",
                UserName: "Kohuz",
                ImageUrl: ""
                );

            CarPoolDbContextSUT.Users.Add(entity);
            await CarPoolDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntities);
        }



    }
}
