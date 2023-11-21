using Microsoft.EntityFrameworkCore;
using CarPool.DAL.Entities;
using CarPool.DAL.Seeds;

namespace CarPool.DAL
{
    // Access to database
    public class CarPoolDbContext : DbContext
    {
        private readonly bool _seedDemoData;

        public CarPoolDbContext(DbContextOptions contextOptions, bool seedDemoData = true)
            : base(contextOptions)
        {
            _seedDemoData = seedDemoData;
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CarEntity> Cars => Set<CarEntity>();
        public DbSet<NumberOfRidesEntity> NumberOfRides => Set<NumberOfRidesEntity>();
        public DbSet<RideEntity> Rides => Set<RideEntity>();
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //if (_seedDemoData)
            //{

                UserSeed.Seed(modelBuilder);
                CarSeed.Seed(modelBuilder);
                RideSeed.Seed(modelBuilder);
                NumberOfRidesSeed.Seed(modelBuilder);
           // }
        }
    }
}
