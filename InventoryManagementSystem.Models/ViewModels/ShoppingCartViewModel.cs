using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
    }

    public class CartItemViewModel
    {
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int MaxQuantity { get; set; } // من الـ Stock
    }
}
