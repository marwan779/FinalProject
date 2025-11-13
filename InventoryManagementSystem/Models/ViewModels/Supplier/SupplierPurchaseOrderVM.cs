using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.ViewModels.Supplier
{
    public class SupplierPurchaseOrderVM
    {

        public int PurchaseOrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
