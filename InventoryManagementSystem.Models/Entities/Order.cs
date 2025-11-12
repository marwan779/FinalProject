using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending"; // Pending, Confirmed, Shipped, Delivered, Cancelled

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Instapay, Wallet

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed

        // Shipping Information
        [Required]
        [MaxLength(100)]
        public string ShippingFullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ShippingPhone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ShippingEmail { get; set; } = string.Empty;

        // Payment tracking
        public string? PaymentIntentId { get; set; } // For Instapay
        public DateTime? PaymentDate { get; set; }

        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Calculated property
        [NotMapped]
        public int ItemCount => OrderItems?.Sum(item => item.Quantity) ?? 0;
    }
}