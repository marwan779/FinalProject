using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        // عرض الـ Cart
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.ShoppingCartRepository.GetUserCart(userId);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems.Select(c => new CartItemViewModel
                {
                    ShoppingCartId = c.ShoppingCartId,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    ProductImage = c.Product.ProductImagePath,
                    UnitPrice = c.Product.UnitPrice,
                    Quantity = c.Quantity,
                    TotalPrice = c.TotalPrice,
                    MaxQuantity = c.Product.QuantityInStock
                }).ToList()
            };

            viewModel.CartTotal = viewModel.CartItems.Sum(c => c.TotalPrice);
            viewModel.CartCount = viewModel.CartItems.Sum(c => c.Quantity);

            return View(viewModel);
        }




        // إضافة منتج للـ Cart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // جلب المنتج
            var products = _unitOfWork.ProductRepository.GetAllWithCategory();
            var product = products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found!" });
            }

            // التأكد من الـ Stock
            if (product.QuantityInStock < quantity)
            {
                return Json(new { success = false, message = "Not enough stock!" });
            }

            // شوف لو المنتج موجود في الـ Cart
            var existingCart = _unitOfWork.ShoppingCartRepository.GetCartItem(userId, productId);

            if (existingCart != null)
            {
                // زود الكمية
                existingCart.Quantity += quantity;

                if (existingCart.Quantity > product.QuantityInStock)
                {
                    return Json(new { success = false, message = "Not enough stock!" });
                }

                _unitOfWork.ShoppingCartRepository.Update(existingCart);
            }
            else
            {
                // أضف عنصر جديد
                var cart = new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.UtcNow
                };
                _unitOfWork.ShoppingCartRepository.Add(cart);
            }

            _unitOfWork.Save();

            var cartCount = _unitOfWork.ShoppingCartRepository.GetCartCount(userId);

            return Json(new
            {
                success = true,
                message = "Product added to cart successfully!",
                cartCount = cartCount
            });
        }




        // تحديث الكمية
        [HttpPost]
        public IActionResult UpdateQuantity(int cartId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.ShoppingCartRepository.GetUserCart(userId);
            var cart = cartItems.FirstOrDefault(c => c.ShoppingCartId == cartId);

            if (cart == null || cart.ApplicationUserId != userId)
            {
                return Json(new { success = false, message = "Cart item not found!" });
            }

            if (quantity > cart.Product.QuantityInStock)
            {
                return Json(new { success = false, message = "Not enough stock!" });
            }

            cart.Quantity = quantity;
            _unitOfWork.ShoppingCartRepository.Update(cart);
            _unitOfWork.Save();

            var allCartItems = _unitOfWork.ShoppingCartRepository.GetUserCart(userId);
            var cartTotal = allCartItems.Sum(c => c.TotalPrice);
            var itemTotal = cart.TotalPrice;

            return Json(new
            {
                success = true,
                itemTotal = itemTotal,
                cartTotal = cartTotal
            });
        }



        // حذف من الـ Cart
        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.ShoppingCartRepository.GetUserCart(userId);
            var cart = cartItems.FirstOrDefault(c => c.ShoppingCartId == cartId);

            if (cart == null || cart.ApplicationUserId != userId)
            {
                return Json(new { success = false, message = "Cart item not found!" });
            }

            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();

            var cartCount = _unitOfWork.ShoppingCartRepository.GetCartCount(userId);

            return Json(new
            {
                success = true,
                message = "Item removed from cart!",
                cartCount = cartCount
            });
        }



        // مسح الـ Cart بالكامل
        [HttpPost]
        public IActionResult Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _unitOfWork.ShoppingCartRepository.ClearCart(userId);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Cart cleared!" });
        }



        // جلب عدد العناصر (للـ Navbar)
        [HttpGet]
        public IActionResult GetCartCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { count = 0 });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = _unitOfWork.ShoppingCartRepository.GetCartCount(userId);

            return Json(new { count = count });
        }



        public IActionResult Checkout()
        {
            return RedirectToAction("Index", "Checkout");
        }


        [AllowAnonymous] // علشان نسمح للجميع بالوصول
        public IActionResult UnauthorizedAccess()
        {
            return View();
        }
    }
}