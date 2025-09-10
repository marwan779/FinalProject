using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entities
{
    public class InventoryTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int QuantityChanged { get; set; }

        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty; // Purchase, Sale, Adjustment

        public int? ReferenceId { get; set; } // Optional link to Purchase/Sale order

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
