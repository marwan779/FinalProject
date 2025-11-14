using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8077));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8080));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8230), new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8231) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8237), new DateTime(2025, 11, 13, 22, 55, 36, 93, DateTimeKind.Utc).AddTicks(8238) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
