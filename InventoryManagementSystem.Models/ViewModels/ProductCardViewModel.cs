using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class ProductCardViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductImagePath { get; set; }
        public string CategoryName { get; set; } // ⬅️ من الـ Category
        public bool IsInStock { get; set; } // ⬅️ هنحسبها من QuantityInStock
    }
}
