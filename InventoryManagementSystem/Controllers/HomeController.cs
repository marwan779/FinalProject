using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventoryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // ⬅️ بدل الـ Repositories المنفصلة

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string? searchTerm, int? categoryId, string sortBy = "name", int page = 1)
        {
            const int pageSize = 12;

            // استخدم _unitOfWork.ProductRepository
            var products = _unitOfWork.ProductRepository.GetFilteredProducts(
                searchTerm,
                categoryId,
                sortBy,
                page,
                pageSize,
                out int totalCount);

            var productCards = products.Select(p => new ProductCardViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                ProductImagePath = p.ProductImagePath,
                CategoryName = p.Category.Name,
                IsInStock = p.QuantityInStock > 0
            }).ToList();

            // استخدم _unitOfWork.CategoryRepository
            var categories = _unitOfWork.CategoryRepository.GetAllCategories()
                .Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList();

            var viewModel = new ProductFilterViewModel
            {
                Products = productCards,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                SortBy = sortBy,
                CurrentPage = page,
                PageSize = pageSize,
                TotalProducts = totalCount,
                Categories = categories
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var product = _unitOfWork.ProductRepository.GetAllWithCategory()
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}