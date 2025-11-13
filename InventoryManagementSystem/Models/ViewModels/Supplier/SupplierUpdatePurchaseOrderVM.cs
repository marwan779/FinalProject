using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Models.ViewModels.Supplier
{
    public class SupplierUpdatePurchaseOrderVM
    {
        public int PurchaseOrderId { get; set; }
        public string CurrentStatus { get; set; }   
    }
}
