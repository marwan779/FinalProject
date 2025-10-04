using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.ViewModels.Product
{
    public class CreateProductVM
    {

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal CostPrice { get; set; }
        [Required]

        public int QuantityInStock { get; set; }
        [Required]

        public IFormFile ProductImage { get; set; }
    }
}
