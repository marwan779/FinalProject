using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.ViewModels.PurchaseOrder
{
    public class UpdatePurchaseOrderVM
    {
        public int PurchaseOrderId { get; set; }
        [Required]
        public string SupplierId { get; set; } = string.Empty;
        public List<SelectListItem> Suppliers { get; set; } = new();
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
