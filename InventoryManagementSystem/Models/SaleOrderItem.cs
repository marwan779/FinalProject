using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models
{
    public class SaleOrderItem
    {
        [Key]
        public int SaleOrderItemId { get; set; }

        [Required]
        public int SaleOrderId { get; set; }
        public SaleOrder SaleOrder { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
    }
}
