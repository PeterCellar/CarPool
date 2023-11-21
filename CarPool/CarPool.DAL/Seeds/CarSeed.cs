using System;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds
{
    public static class CarSeed
    {
        public static readonly CarEntity Fiat = new(
            Id: Guid.Parse(input: "3A8490B2-3BD3-410D-A513-6340699E9FA5"),
            Manufacturer: "Audi",
            Type: "A7",
            FirstRegistration: new DateTime(2018, 2, 1),
            Seats: 5,
            OwnerId: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
            ImageUrl: "");

        public static readonly CarEntity Renault = new(
           Id: Guid.Parse(input: "7D33C854-2B3A-4B4E-8B7C-A340803977AD"),
           Manufacturer: "Mustang",
           Type: "GT",
           FirstRegistration: new DateTime(2018, 5, 30),
           Seats: 5,
           OwnerId: Guid.Parse(input: "AC3D462E-8DA6-46AB-B156-30688E44917A"),
           ImageUrl: "");



        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarEntity>().HasData(
                Fiat,
                Renault
            ) ;
        }
    }
}