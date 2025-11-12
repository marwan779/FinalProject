using InventoryManagementSystem.Models.Entities;
using System.Collections.Generic;

namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        Order GetOrderById(int orderId);
        Order GetOrderWithItems(int orderId);
        IEnumerable<Order> GetUserOrders(string userId);
        IEnumerable<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);
        void UpdatePaymentStatus(int orderId, string paymentStatus, string? paymentIntentId = null);

        // Methods جديدة
        void UpdateProductStock(int orderId);
        void CancelOrder(int orderId);
    }
}