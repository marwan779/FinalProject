using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;

namespace InventoryManagementSystem.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessInstapayPaymentAsync(Order order);
        Task<bool> VerifyInstapayPaymentAsync(string paymentIntentId);
        Task<PaymentResult> ProcessWalletPaymentAsync(Order order);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}