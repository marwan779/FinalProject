using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entities
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Required]
        public string SupplierId { get; set; } = string.Empty;
        public ApplicationUser Supplier { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
