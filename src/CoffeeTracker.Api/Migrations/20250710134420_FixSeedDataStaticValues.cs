using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataStaticValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4360));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4363));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4365));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4366));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4368));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4369));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4370));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4372));

            migrationBuilder.UpdateData(
                table: "CoffeeShops",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 10, 13, 43, 15, 918, DateTimeKind.Utc).AddTicks(4372));
        }
    }
}
