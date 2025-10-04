using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


        [HttpGet]
        public IActionResult CreateProduct()
        {
            try
            {
                CreateProductVM createProductVM = new CreateProductVM()
                {
                    Categories = _unitOfWork.CategoryRepository.GetAll()
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList()
                };

                return View(createProductVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(new CreateProductVM());
            }
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductVM createProductVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createProductVM);
                }

                Product product = new Product()
                {
                    Name = createProductVM.Name,
                    Description = createProductVM.Description,
                    CategoryId = createProductVM.CategoryId,
                    UnitPrice = createProductVM.UnitPrice,
                    CostPrice = createProductVM.CostPrice,
                    QuantityInStock = createProductVM.QuantityInStock,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                    // ProductImagePath will be set later after you upload or process the image
                };


                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(createProductVM);
            }
        }

    }
}
