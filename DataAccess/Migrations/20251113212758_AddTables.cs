using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(615));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(619));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(746), new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(746) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(752), new DateTime(2025, 11, 13, 21, 27, 56, 181, DateTimeKind.Utc).AddTicks(753) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(8913));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(9036), new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(9036) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(9042), new DateTime(2025, 11, 12, 16, 53, 46, 752, DateTimeKind.Utc).AddTicks(9042) });
        }
    }
}
