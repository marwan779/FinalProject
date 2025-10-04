using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Models.ViewModels.Product
{
    public class UpdateProductVM 
    { 
        public int ProductId { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string? Description { get; set; } = string.Empty; 
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public decimal UnitPrice { get; set; } 
        public decimal CostPrice { get; set; } 
        public int QuantityInStock { get; set; }
        [ValidateNever]
        public IFormFile ProductImage { get; set; } 
        public string ProductImagePath { get; set; } = string.Empty; 
    }
}
