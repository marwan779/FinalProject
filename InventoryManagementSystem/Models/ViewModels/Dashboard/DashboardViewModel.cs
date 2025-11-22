using System;
using System.Collections.Generic;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        // الكروت العلوية (KPI Cards)
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int TodayTransactions { get; set; }
        public decimal TotalStockValue { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }

        // للرسوم البيانية
        public List<CategoryStock> CategoriesStock { get; set; } = new List<CategoryStock>();
        public List<WeeklyTransaction> WeeklyTransactions { get; set; } = new List<WeeklyTransaction>();

        // القوائم الحديثة
        public List<TransactionSummary> RecentTransactions { get; set; } = new List<TransactionSummary>();
        public List<LowStockProduct> LowStockProducts { get; set; } = new List<LowStockProduct>();
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
    }

    public class CategoryStock
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class WeeklyTransaction
    {
        public string Day { get; set; } = string.Empty;
        public int StockIn { get; set; }
        public int StockOut { get; set; }
    }

    public class TransactionSummary
    {
        public string ProductName { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public int QuantityChanged { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BadgeClass
        {
            get
            {
                return TransactionType.ToUpper() == "IN" ? "badge-success" : "badge-warning";
            }
        }
        public string Icon
        {
            get
            {
                return TransactionType.ToUpper() == "IN" ? "📥" : "📤";
            }
        }
    }

    public class LowStockProduct
    {
        public string ProductName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
    }

    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}