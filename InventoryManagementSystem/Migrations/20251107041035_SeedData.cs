using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CostPrice", "CreatedAt", "Description", "Name", "ProductImagePath", "UnitPrice", "UpdatedAt" },
                values: new object[] { 12000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9642), "High performance laptop", "Laptop", "/images/laptop.jpg", 15000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9642) });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "CostPrice", "CreatedAt", "Description", "Name", "ProductImagePath", "QuantityInStock", "UnitPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9646), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9647) },
                    { 3, 1, 12000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9648), "High performance laptop", "Laptop", "/images/laptop.jpg", 10, 15000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9649) },
                    { 4, 1, 12000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9651), "High performance laptop", "Laptop", "/images/laptop.jpg", 10, 15000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9651) },
                    { 5, 1, 12000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9653), "High performance laptop", "Laptop", "/images/laptop.jpg", 10, 15000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9653) },
                    { 6, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9655), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9655) },
                    { 7, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9657), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9657) },
                    { 8, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9659), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9660) },
                    { 9, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9661), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9662) },
                    { 10, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9663), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9664) },
                    { 11, 2, 6000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9666), "Latest smartphone", "Smartphone", "/images/phone.jpg", 20, 8000.00m, new DateTime(2025, 11, 7, 4, 10, 33, 353, DateTimeKind.Utc).AddTicks(9666) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 6, 23, 40, 1, 649, DateTimeKind.Utc).AddTicks(8447));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 6, 23, 40, 1, 649, DateTimeKind.Utc).AddTicks(8450));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CostPrice", "CreatedAt", "Description", "Name", "ProductImagePath", "UnitPrice", "UpdatedAt" },
                values: new object[] { 18000.00m, new DateTime(2025, 11, 6, 23, 40, 1, 649, DateTimeKind.Utc).AddTicks(8827), "Professional espresso coffee machine.", "Espresso Machine", "Images/Products/test.jpg", 25000.00m, new DateTime(2025, 11, 6, 23, 40, 1, 649, DateTimeKind.Utc).AddTicks(8828) });
        }
    }
}
