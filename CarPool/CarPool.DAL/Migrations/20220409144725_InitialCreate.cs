using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPool.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedCarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rides_Cars_UsedCarId",
                        column: x => x.UsedCarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rides_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NumberOfRides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberOfRides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumberOfRides_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NumberOfRides_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageUrl", "Name", "Surname", "UserName" },
                values: new object[] { new Guid("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"), "", "martin", "novák", "pac" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageUrl", "Name", "Surname", "UserName" },
                values: new object[] { new Guid("ac3d462e-8da6-46ab-b156-30688e44917a"), "", "josef", "malý", "man" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ImageUrl", "Name", "Surname", "UserName" },
                values: new object[] { new Guid("ced8bfbb-93ee-49f2-a0c6-df9b920e861a"), "", "Jakub", "Vlk", "banan" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "FirstRegistration", "ImageUrl", "Manufacturer", "OwnerId", "Seats", "Type" },
                values: new object[] { new Guid("3a8490b2-3bd3-410d-a513-6340699e9fa5"), new DateTime(2000, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Fiat", new Guid("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"), 5, "Multipla" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "FirstRegistration", "ImageUrl", "Manufacturer", "OwnerId", "Seats", "Type" },
                values: new object[] { new Guid("7d33c854-2b3a-4b4e-8b7c-a340803977ad"), new DateTime(2000, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "renault", new Guid("ac3d462e-8da6-46ab-b156-30688e44917a"), 5, "laguna" });

            migrationBuilder.InsertData(
                table: "Rides",
                columns: new[] { "Id", "DriverId", "EndLocation", "EndTime", "StartLocation", "StartTime", "UsedCarId" },
                values: new object[] { new Guid("5d38c1dd-0cf1-42cd-925c-5e8dea737a2a"), new Guid("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"), "Misto B", new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Misto A", new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3a8490b2-3bd3-410d-a513-6340699e9fa5") });

            migrationBuilder.InsertData(
                table: "NumberOfRides",
                columns: new[] { "Id", "RideId", "UserId" },
                values: new object[] { new Guid("1422ca21-c6ff-413e-8070-fa8a896a1f8d"), new Guid("5d38c1dd-0cf1-42cd-925c-5e8dea737a2a"), new Guid("ac3d462e-8da6-46ab-b156-30688e44917a") });

            migrationBuilder.InsertData(
                table: "NumberOfRides",
                columns: new[] { "Id", "RideId", "UserId" },
                values: new object[] { new Guid("b5bf3a8e-c312-4dda-9d1f-9f6fd68e7c31"), new Guid("5d38c1dd-0cf1-42cd-925c-5e8dea737a2a"), new Guid("ced8bfbb-93ee-49f2-a0c6-df9b920e861a") });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_OwnerId",
                table: "Cars",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberOfRides_RideId",
                table: "NumberOfRides",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_NumberOfRides_UserId",
                table: "NumberOfRides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_UsedCarId",
                table: "Rides",
                column: "UsedCarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberOfRides");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
