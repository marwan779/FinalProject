using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.ViewModels.InventoryTransaction
{
    public class InventoryTransactionVM
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int QuantityChanged { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public int? ReferenceId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
