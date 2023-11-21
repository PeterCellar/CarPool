using System;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class RideSeed
{
    public static readonly RideEntity Ride1 = new(
        Id: Guid.Parse(input: "5D38C1DD-0CF1-42CD-925C-5E8DEA737A2A"),
        StartTime: new DateTime(2023, 5, 5, 17, 0, 0),
        EndTime: new DateTime(2023, 5, 5, 19, 0, 0),
        StartLocation: "Brno",
        EndLocation: "Praha",
        UsedCarId: Guid.Parse(input: "3A8490B2-3BD3-410D-A513-6340699E9FA5"),
        DriverId: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75")
        );

    public static readonly RideEntity Ride2 = new(
       Id: Guid.Parse(input: "42042042-0CF1-42CD-925C-5E8DEA737A2A"),
       StartTime: new DateTime(2023, 1, 1, 3, 0, 0),
       EndTime: new DateTime(2023, 1, 1, 6, 0, 0),
       StartLocation: "Ostrava",
       EndLocation: "Bratislava",
       UsedCarId: Guid.Parse(input: "3A8490B2-3BD3-410D-A513-6340699E9FA5"),
       DriverId: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75")
       );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            Ride1
        );
        modelBuilder.Entity<RideEntity>().HasData(
            Ride2
        );
    }
}