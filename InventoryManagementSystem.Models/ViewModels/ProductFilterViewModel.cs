using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        // Search & Filter Parameters
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "name"; // name, price_asc, price_desc

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9; // عدد المنتجات في الصفحة
        public int TotalProducts { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);

        // Categories للـ Dropdown
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<ProductCardViewModel> Products { get; set; }


       

    }
}
