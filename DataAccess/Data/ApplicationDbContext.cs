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
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);




            // Relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



            // ---- Seed Main Categories ----
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Laptops" },
                new Category { CategoryId = 2, Name = "Mobiles" }
            );

            modelBuilder.Entity<Product>().HasData(
        new Product
        {
            ProductId = 1,
            Name = "Laptop",
            Description = "High performance laptop",
            CategoryId = 1,
            UnitPrice = 15000.00m,
            CostPrice = 12000.00m,
            QuantityInStock = 10,
            ProductImagePath = "/images/laptop.jpg"
        },
        new Product
        {
            ProductId = 2,
            Name = "Smartphone",
            Description = "Latest smartphone",
            CategoryId = 2,
            UnitPrice = 8000.00m,
            CostPrice = 6000.00m,
            QuantityInStock = 20,
            ProductImagePath = "/images/phone.jpg"
        }
       
    );




        }

    }


}
