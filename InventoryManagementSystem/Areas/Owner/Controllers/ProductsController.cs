using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            try
            {
                List<Product> products = _unitOfWork.ProductRepository.GetAll(IncludeProperties: "Category,").ToList();
                List<ProductVM> productsVM = new List<ProductVM>();

                if (products.Any())
                {
                    foreach (var p in products)
                    {
                        ProductVM vm = new ProductVM
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            Description = p.Description,
                            CategoryId = p.CategoryId,
                            CategoryName = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == p.CategoryId).Name,
                            UnitPrice = p.UnitPrice,
                            CostPrice = p.CostPrice,
                            QuantityInStock = p.QuantityInStock,
                            IsDamaged = p.IsDamaged,
                            CreatedAt = p.CreatedAt,
                            UpdatedAt = p.UpdatedAt,
                            ProductImagePath = p.ProductImagePath
                        };

                        productsVM.Add(vm);
                    }
                }

                return View(productsVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }
    }
}
