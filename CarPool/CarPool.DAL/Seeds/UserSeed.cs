using System;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class UserSeed

{
    public static readonly UserEntity Josef = new(
       Id: Guid.Parse(input: "AC3D462E-8DA6-46AB-B156-30688E44917A"),
       UserName: "Kiwi",
       Surname: "Malý",
       Name: "Josef",
       ImageUrl: "");

    public static readonly UserEntity Martin = new(
        Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        UserName: "Jahodka",
        Surname: "Novák",
        Name: "Martin",
        ImageUrl: "");
    public static readonly UserEntity Jakub = new(
        Id: Guid.Parse(input: "CED8BFBB-93EE-49F2-A0C6-DF9B920E861A"),
        UserName: "Banan",
        Surname: "Vlk",
        Name: "Jakub",
        ImageUrl: "");



    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            Martin,
            Josef,
            Jakub
        );

    }
}