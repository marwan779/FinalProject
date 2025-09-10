using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entities
{
    public class SaleOrder
    {
        [Key]
        public int SaleOrderId { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;
        public ApplicationUser Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<SaleOrderItem> Items { get; set; } = new List<SaleOrderItem>();
    }
}
