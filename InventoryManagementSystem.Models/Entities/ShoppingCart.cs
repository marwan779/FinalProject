using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models.Entities
{
    public class ShoppingCart
    {

        [Key]
        public int ShoppingCartId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Calculated Property (مش في الـ Database)
        [NotMapped]
        public decimal TotalPrice => Product?.UnitPrice * Quantity ?? 0;
    }
}
