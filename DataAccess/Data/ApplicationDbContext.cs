using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderItem> SaleOrderItems { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Seed Main Categories ----
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Laptops" },
                new Category { CategoryId = 2, Name = "Mobiles" }
            );

            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                Name = "Espresso Machine",
                Description = "Professional espresso coffee machine.",
                CategoryId = 1,
                UnitPrice = 25000.00m,
                CostPrice = 18000.00m,
                QuantityInStock = 10,
                ProductImagePath = "Images/Products/test.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
            );

        }

    }


}
