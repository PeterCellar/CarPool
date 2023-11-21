
using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.DAL.Seeds;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarPool.BL.Tests
{
    public sealed class UserFacadeTests : CRUDFacadeTestsBase
    {
 
        private readonly UserFacade _userFacadeSUT;

        public UserFacadeTests() : base()
        {
            _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task GetAll_Single_SeededMartin()
        {
            var users = await _userFacadeSUT.GetAsync();
            var retrieved = users.Single(i => i.Id == UserSeed.Martin.Id);

            DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeed.Martin), retrieved);
        }

        /*
        * In the next tests the seeded is pulled out of the db
        * so that we can include navigation properties,
        * then compare with mapped model from Facade
        */

        [Fact]
        public async Task Add_user_()
        {

            //Arange
            var user = new UserDetailModel(
                    UserName: "Mickey",
                    Name: "Petr",
                    Surname: "Novák");

            //Act
            user = await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.Users.SingleAsync(i => i.Id == user.Id);
            var mapped = Mapper.Map<UserDetailModel>(userFromDb);

            DeepAssert.Equal(user, mapped);
        }

        [Fact]
        public async Task GetById_SeededMartin()
        {

            //Arange
            var detailModel = Mapper.Map<UserDetailModel>(UserSeed.Martin);

            //Act
            var returnedModel = await _userFacadeSUT.GetAsync(detailModel.Id);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var users = await dbxAssert.Users
                .Include(b => b.CarsOwned)
                .Include(c => c.RidesTaking)
                .Include(d => d.Offering)
                .ToArrayAsync();
            var userFromdb = users.Single(i => i.Id == UserSeed.Martin.Id);
            var userModel = Mapper.Map<UserDetailModel>(userFromdb);
            DeepAssert.Equal(userModel, returnedModel);
        }

        [Fact]
        public async Task SeededMartin_DeleteById_Deleted_cascade()
        {
            await _userFacadeSUT.DeleteAsync(UserSeed.Martin.Id);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == UserSeed.Martin.Id));
            Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeed.Fiat.Id));
            Assert.False(await dbxAssert.Rides.AnyAsync(i => i.Id == RideSeed.Ride1.Id));
        }
        [Fact]
        public async Task Getall_checkContainsJakub()
        {
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var users = await dbxAssert.Users.ToArrayAsync();
            var userFromDb = users.Single(i => i.Id == UserSeed.Jakub.Id);
            var userModel = Mapper.Map<UserListModel>(userFromDb);

            var returnedModel = await _userFacadeSUT.GetAsync();

            Assert.Contains(userModel, returnedModel);
        }



    }
}
