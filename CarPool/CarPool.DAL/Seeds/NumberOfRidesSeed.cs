using System;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds
{

    public static class NumberOfRidesSeed
    {
        public static readonly NumberOfRidesEntity RideRelationship1 = new(
            Id: Guid.Parse(input: "1422CA21-C6FF-413E-8070-FA8A896A1F8D"),
            UserId: Guid.Parse(input: "AC3D462E-8DA6-46AB-B156-30688E44917A"),
            RideId: Guid.Parse(input: "5D38C1DD-0CF1-42CD-925C-5E8DEA737A2A"))
        {
            User = UserSeed.Josef,
            Ride = RideSeed.Ride1
        };
        public static readonly NumberOfRidesEntity RideRelationship2 = new(
            Id: Guid.Parse(input: "B5BF3A8E-C312-4DDA-9D1F-9F6FD68E7C31"),
            UserId: Guid.Parse(input: "CED8BFBB-93EE-49F2-A0C6-DF9B920E861A"),
            RideId: Guid.Parse(input: "5D38C1DD-0CF1-42CD-925C-5E8DEA737A2A"))
        {
            User = UserSeed.Jakub,
            Ride = RideSeed.Ride1
        };


        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NumberOfRidesEntity>().HasData(
               RideRelationship1 with { User = null, Ride = null },
               RideRelationship2 with { User = null, Ride = null }
            );

        }
    }
}