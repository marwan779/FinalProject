using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsDamaged { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string ProductImagePath { get; set; } = string.Empty;

    }
}
