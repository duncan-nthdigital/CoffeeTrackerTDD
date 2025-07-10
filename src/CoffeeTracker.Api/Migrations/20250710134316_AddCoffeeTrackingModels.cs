using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeTrackingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoffeeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "Type of coffee consumed (e.g., Espresso, Latte)"),
                    Size = table.Column<string>(type: "TEXT", nullable: false, comment: "Size of the coffee (e.g., Small, Medium, Large)"),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true, comment: "Source of the coffee (e.g., coffee shop name)"),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')", comment: "When the coffee was consumed (UTC)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "Name of the coffee shop"),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true, comment: "Address of the coffee shop"),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true, comment: "Whether the coffee shop is active"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')", comment: "When the coffee shop record was created (UTC)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShops", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CoffeeShops",
                columns: new[] { "Id", "Address", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4360), true, "Home" },
                    { 2, "Multiple Locations", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4363), true, "Starbucks" },
                    { 3, "Multiple Locations", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4365), true, "Dunkin' Donuts" },
                    { 4, "123 Main Street", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4366), true, "Local Coffee House" },
                    { 5, "456 Oak Avenue", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4368), true, "Peet's Coffee" },
                    { 6, "789 Elm Street", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4369), true, "The Coffee Bean & Tea Leaf" },
                    { 7, "321 Pine Road", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4370), true, "Blue Bottle Coffee" },
                    { 8, "654 Maple Drive", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4372), true, "Tim Hortons" },
                    { 9, "987 Cedar Lane", new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4372), true, "Costa Coffee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeEntries_Timestamp",
                table: "CoffeeEntries",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShops_IsActive",
                table: "CoffeeShops",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShops_Name",
                table: "CoffeeShops",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeEntries");

            migrationBuilder.DropTable(
                name: "CoffeeShops");
        }
    }
}
