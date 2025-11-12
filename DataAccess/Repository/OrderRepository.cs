using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public Order GetOrderWithItems(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.ApplicationUser)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public IEnumerable<Order> GetUserOrders(string userId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.ApplicationUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.ApplicationUser)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.OrderStatus = status;
                _context.Orders.Update(order);
            }
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus, string? paymentIntentId = null)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.PaymentStatus = paymentStatus;
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    order.PaymentIntentId = paymentIntentId;
                }
                if (paymentStatus == "Paid")
                {
                    order.PaymentDate = DateTime.UtcNow;
                }
                _context.Orders.Update(order);
            }
        }

        // Method جديدة: تحديث الـ Stock بعد الطلب
        public void UpdateProductStock(int orderId)
        {
            var order = GetOrderWithItems(orderId);
            if (order != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.QuantityInStock -= item.Quantity;
                        _context.Products.Update(product);
                    }
                }
            }
        }

        // Method جديدة: إلغاء الطلب واستعادة الـ Stock
        public void CancelOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null && order.OrderStatus != "Cancelled")
            {
                order.OrderStatus = "Cancelled";

                // استعادة الـ Stock
                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.QuantityInStock += item.Quantity;
                        _context.Products.Update(product);
                    }
                }

                _context.Orders.Update(order);
            }
        }
    }
}