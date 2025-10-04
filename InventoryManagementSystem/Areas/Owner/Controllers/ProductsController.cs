using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.Product;
using InventoryManagementSystem.Services.ImageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public ProductsController(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = _unitOfWork.ProductRepository.GetAll(IncludeProperties: "Category,").ToList();
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
        public async Task<IActionResult> CreateProduct(CreateProductVM createProductVM)
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
                };

                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();

                string? relativePath = await _imageService.SaveImageAsync(createProductVM.ProductImage, $"Images/Products/{product.ProductId.ToString()}");
                
                if(!String.IsNullOrEmpty(relativePath)) 
                    product.ProductImagePath = relativePath;

                _unitOfWork.ProductRepository.Update(product);

                _unitOfWork.Save();


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
