using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.Product;
using InventoryManagementSystem.Services.ImageService;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Linq.Expressions;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = StaticDetails.OwnerRole)]

    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = _unitOfWork.ProductRepository.GetAll(IncludeProperties: "Category").ToList();
                List<ProductVM> productsVM = new List<ProductVM>();


                List<Category> categories = _unitOfWork.CategoryRepository.GetAll().ToList();

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
                return RedirectToAction(nameof(Index));
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

                if (!StaticDetails.allowedImageExtensions.Contains(Path.GetExtension(createProductVM.ProductImage.FileName)))
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
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult UpdateProduct(int productId)
        {
            try
            {
                Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == productId);

                UpdateProductVM updateProductVM = new UpdateProductVM
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    CostPrice = product.CostPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductImagePath = product.ProductImagePath,
                    CategoryId = product.CategoryId,
                    Categories = _unitOfWork.CategoryRepository.GetAll()
                        .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                        .ToList()
                };

                return View(updateProductVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductVM updateProductVM)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(updateProductVM);
                }

                Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == updateProductVM.ProductId);
                product.Name = updateProductVM.Name;
                product.Description = updateProductVM.Description;
                product.CategoryId = updateProductVM.CategoryId;
                product.UnitPrice = updateProductVM.UnitPrice;
                product.CostPrice = updateProductVM.CostPrice;
                product.QuantityInStock = updateProductVM.QuantityInStock;
                product.UpdatedAt = DateTime.UtcNow;

                if (updateProductVM.ProductImage != null)
                {
                    _imageService.DeleteImage(product.ProductImagePath, "");

                    string? relativePath = await _imageService.SaveImageAsync(
                        updateProductVM.ProductImage,
                        $"Images/Products/{product.ProductId}"
                    );

                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        product.ProductImagePath = relativePath;
                    }
                }

                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == productId);

                string? productImagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", product.ProductId.ToString());

                if (Directory.Exists(productImagesFolder))
                {
                    Directory.Delete(productImagesFolder, true);
                }

                _unitOfWork.ProductRepository.Remove(product);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
