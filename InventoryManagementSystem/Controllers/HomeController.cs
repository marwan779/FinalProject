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



        public IActionResult Dashboard()
        {
            try
            {
                var dashboardData = new DashboardViewModel
                {
                    // الكروت العلوية الأساسية
                    TotalProducts = _unitOfWork.ProductRepository.GetAll().Count(),
                    TotalSuppliers = _unitOfWork.PurchaseOrderRepository.GetAll()
                                    .Select(po => po.SupplierId).Distinct().Count(),
                    LowStockItems = _unitOfWork.ProductRepository.GetAll()
                                    .Where(p => p.QuantityInStock > 0 && p.QuantityInStock < 10).Count(),
                    OutOfStockItems = _unitOfWork.ProductRepository.GetAll()
                                      .Where(p => p.QuantityInStock == 0).Count(),
                    TodayTransactions = _unitOfWork.InventoryTransactionRepository.GetAll()
                                        .Count(t => t.CreatedAt.Date == DateTime.Today),
                    TotalStockValue = _unitOfWork.ProductRepository.GetAll()
                                     .Sum(p => p.QuantityInStock * p.CostPrice)
                };

                // بيانات الفئات للرسم البياني
                var categories = _unitOfWork.CategoryRepository.GetAll().ToList();
                dashboardData.CategoriesStock = categories.Select(c => new CategoryStock
                {
                    CategoryName = c.Name,
                    ProductCount = _unitOfWork.ProductRepository.GetAll()
                                  .Count(p => p.CategoryId == c.CategoryId),
                    Color = GetCategoryColor(c.Name)
                }).Where(cs => cs.ProductCount > 0).ToList();

                // المنتجات المنخفضة المخزون
                var lowStockProducts = _unitOfWork.ProductRepository.GetAll()
                                      .Where(p => p.QuantityInStock > 0 && p.QuantityInStock < 10)
                                      .OrderBy(p => p.QuantityInStock)
                                      .Take(5)
                                      .ToList();

                dashboardData.LowStockProducts = lowStockProducts.Select(p => new LowStockProduct
                {
                    ProductName = p.Name,
                    CurrentStock = p.QuantityInStock
                }).ToList();

                // بيانات الأسبوع المبسطة
                dashboardData.WeeklyTransactions = GetSimpleWeeklyData();

                // المعاملات الحديثة المبسطة
                dashboardData.RecentTransactions = GetSimpleRecentTransactions();

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                // لو في أي error، نرجع dashboard فاضي عشان ما يقعش التطبيق
                return View(new DashboardViewModel());
            }
        }

        // دالة مبسطة لبيانات الأسبوع
        private List<WeeklyTransaction> GetSimpleWeeklyData()
        {
            var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            var random = new Random();

            return days.Select(day => new WeeklyTransaction
            {
                Day = day,
                StockIn = random.Next(5, 20),
                StockOut = random.Next(3, 15)
            }).ToList();
        }

        // دالة مبسطة للمعاملات الحديثة
        private List<TransactionSummary> GetSimpleRecentTransactions()
        {
            var products = _unitOfWork.ProductRepository.GetAll().Take(5).ToList();
            var random = new Random();

            return products.Select(p => new TransactionSummary
            {
                ProductName = p.Name,
                TransactionType = random.Next(0, 2) == 0 ? "IN" : "OUT",
                QuantityChanged = random.Next(1, 10),
                CreatedAt = DateTime.Now.AddHours(-random.Next(1, 24))
            }).ToList();
        }

        // دالة الألوان
        private string GetCategoryColor(string categoryName)
        {
            return categoryName.ToLower() switch
            {
                "laptops" => "#4CAF50",
                "mobiles" => "#2196F3",
                "accessories" => "#FF9800",
                _ => "#9C27B0"
            };
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