using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;
using InventoryManagementSystem.Models.ViewModels.PurchaseOrder;
using InventoryManagementSystem.Services.PaymentService;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public CheckoutController(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }




        // GET: /Checkout
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // جلب بيانات الـ Cart
                var cartItems = _unitOfWork.ShoppingCartRepository.GetUserCart(userId);

                if (cartItems == null || !cartItems.Any())
                {
                    TempData["error"] = "Your cart is empty!";
                    return RedirectToAction("Index", "Cart");
                }

                // التأكد من الـ Stock
                foreach (var item in cartItems)
                {
                    if (item.Quantity > item.Product.QuantityInStock)
                    {
                        TempData["error"] = $"{item.Product.Name} doesn't have enough stock!";
                        return RedirectToAction("Index", "Cart");
                    }
                }

                // جلب بيانات المستخدم
                var user = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

                // عمل الـ ViewModel
                var viewModel = new CheckoutViewModel
                {
                    CartItems = cartItems.Select(c => new CartItemViewModel
                    {
                        ProductId = c.ProductId,
                        ProductName = c.Product.Name,
                        ProductImage = c.Product.ProductImagePath,
                        UnitPrice = c.Product.UnitPrice,
                        Quantity = c.Quantity,
                        TotalPrice = c.TotalPrice,
                        MaxQuantity = c.Product.QuantityInStock
                    }).ToList(),

                    CartTotal = cartItems.Sum(c => c.TotalPrice),
                    CartCount = cartItems.Sum(c => c.Quantity),

                    // Pre-fill user data
                    UserEmail = user?.Email,
                    UserFullName = user?.FullName,
                    ShippingEmail = user?.Email,
                    ShippingFullName = user?.FullName,
                    ShippingPhone = user?.PhoneNumber
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while loading checkout page.";
                return RedirectToAction("Index", "Cart");
            }
        }




        // POST: /Checkout/ProcessOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessOrder(CheckoutViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {


                    // إطبع الأخطاء في الـ Console
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Model Error: {error.ErrorMessage}");
                    }



                    // إعادة تحميل بيانات الـ Cart لو في أخطاء
                    var userIdForReload = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var cartItemsForReload = _unitOfWork.ShoppingCartRepository.GetUserCart(userIdForReload);

                    model.CartItems = cartItemsForReload.Select(c => new CartItemViewModel
                    {
                        ProductId = c.ProductId,
                        ProductName = c.Product.Name,
                        ProductImage = c.Product.ProductImagePath,
                        UnitPrice = c.Product.UnitPrice,
                        Quantity = c.Quantity,
                        TotalPrice = c.TotalPrice,
                        MaxQuantity = c.Product.QuantityInStock
                    }).ToList();

                    model.CartTotal = cartItemsForReload.Sum(c => c.TotalPrice);
                    model.CartCount = cartItemsForReload.Sum(c => c.Quantity);

                    return View("Index", model);
                }

                var userIdForProcessing = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItemsForProcessing = _unitOfWork.ShoppingCartRepository.GetUserCart(userIdForProcessing);

                if (cartItemsForProcessing == null || !cartItemsForProcessing.Any())
                {
                    TempData["error"] = "Your cart is empty!";
                    return RedirectToAction("Index", "Cart");
                }

                // عمل Order جديد
                var order = new Order
                {
                    ApplicationUserId = userIdForProcessing,
                    OrderDate = DateTime.UtcNow,
                    OrderTotal = cartItemsForProcessing.Sum(c => c.TotalPrice),
                    OrderStatus = "Pending",
                    PaymentMethod = model.PaymentMethod,
                    PaymentStatus = model.PaymentMethod == "Cash" ? "Pending" : "Unpaid",

                    // Shipping Information
                    ShippingFullName = model.ShippingFullName,
                    ShippingAddress = model.ShippingAddress,
                    ShippingPhone = model.ShippingPhone,
                    ShippingEmail = model.ShippingEmail
                };

                // إضافة الـ Order Items
                foreach (var cartItem in cartItemsForProcessing)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.Product.Name,
                        UnitPrice = cartItem.Product.UnitPrice,
                        Quantity = cartItem.Quantity,
                        TotalPrice = cartItem.TotalPrice,
                        ProductImagePath = cartItem.Product.ProductImagePath
                    };

                    order.OrderItems.Add(orderItem);
                }

                // حفظ الـ Order
                _unitOfWork.OrderRepository.AddOrder(order);
                _unitOfWork.Save();

                // ⭐⭐⭐ تحديث الـ Stock للدفع الكاش مباشرة ⭐⭐⭐
                if (model.PaymentMethod == "Cash")
                {
                    _unitOfWork.OrderRepository.UpdateProductStock(order.OrderId);
                    _unitOfWork.Save();
                }

                // Redirect بناءً على طريقة الدفع
                if (model.PaymentMethod == "Cash")
                {
                    // دفع كاش - مباشر
                    return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
                }
                else if (model.PaymentMethod == "Instapay")
                {
                    // دفع إلكتروني - هنعمله في الجزء الثالث
                    return RedirectToAction("ProcessElectronicPayment", new { orderId = order.OrderId });
                }
                else if (model.PaymentMethod == "Wallet")
                {
                    // محفظة - هنعمله في الجزء الثالث
                    return RedirectToAction("ProcessWalletPayment", new { orderId = order.OrderId });
                }

                TempData["error"] = "Invalid payment method selected!";
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while processing your order.";
                return View("Index", model);
            }
        }

        // GET: /Checkout/OrderConfirmation
        public IActionResult OrderConfirmation(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.OrderRepository.GetOrderWithItems(orderId);

            if (order == null || order.ApplicationUserId != userId)
            {
                TempData["error"] = "Order not found!";
                return RedirectToAction("Index", "Home");
            }

            // ⭐⭐⭐ تحديث الـ Stock لو كان الدفع إلكتروني واتأكد ⭐⭐⭐
            if (order.PaymentMethod != "Cash" && order.PaymentStatus == "Paid")
            {
                _unitOfWork.OrderRepository.UpdateProductStock(orderId);
            }

            // تنظيف الـ Cart بعد التأكيد
            _unitOfWork.ShoppingCartRepository.ClearCart(userId);
            _unitOfWork.Save();

            return View(order);
        }

        // GET: /Checkout/PaymentSuccess
        public IActionResult PaymentSuccess(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.OrderRepository.GetOrderWithItems(orderId);

            if (order == null || order.ApplicationUserId != userId)
            {
                TempData["error"] = "Order not found!";
                return RedirectToAction("Index", "Home");
            }

            // ⭐⭐⭐ تحديث الـ Stock للدفع الإلكتروني الناجح ⭐⭐⭐
            _unitOfWork.OrderRepository.UpdateProductStock(orderId);


            /*======================== Transaction Part ========================*/

            List<InventoryTransaction> inventoryTransactions = new List<InventoryTransaction>();

            foreach (var item in order.OrderItems)
            {
                InventoryTransaction inventoryTransaction = new InventoryTransaction()
                {
                    ProductId = item.ProductId,
                    QuantityChanged = item.Quantity,
                    TransactionType = StaticDetails.SaleTransaction,
                    ReferenceId = order.OrderId
                };

                _unitOfWork.InventoryTransactionRepository.Add(inventoryTransaction);
            }


            // تنظيف الـ Cart
            _unitOfWork.ShoppingCartRepository.ClearCart(userId);
            _unitOfWork.Save();

            TempData["success"] = "Payment completed successfully!";
            return View("OrderConfirmation", order);
        }



        // GET: /Checkout/PaymentFailed
        public IActionResult PaymentFailed(int orderId)
        {
            var order = _unitOfWork.OrderRepository.GetOrderById(orderId);

            if (order != null)
            {
                order.OrderStatus = "Cancelled";
                order.PaymentStatus = "Failed";
                _unitOfWork.OrderRepository.UpdateOrder(order);
                _unitOfWork.Save();
            }

            TempData["error"] = "Payment failed! Please try again.";
            return RedirectToAction("Index", "Cart");
        }




        // GET: /Checkout/ProcessElectronicPayment
        [HttpGet]
        public async Task<IActionResult> ProcessElectronicPayment(int orderId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var order = _unitOfWork.OrderRepository.GetOrderWithItems(orderId);

                if (order == null || order.ApplicationUserId != userId)
                {
                    TempData["error"] = "Order not found!";
                    return RedirectToAction("Index", "Home");
                }

                // معالجة الدفع عبر انستاباي
                var paymentResult = await _paymentService.ProcessInstapayPaymentAsync(order);

                if (paymentResult.Success)
                {
                    // تحديث الـ Order بالـ Payment Intent ID
                    order.PaymentIntentId = paymentResult.PaymentIntentId;
                    _unitOfWork.OrderRepository.UpdateOrder(order);
                    _unitOfWork.Save();

                    // Redirect لصفحة الدفع
                    if (paymentResult.PaymentUrl.StartsWith("/"))
                    {
                        // Mock payment
                        return Redirect(paymentResult.PaymentUrl);
                    }
                    else
                    {
                        // Real payment - redirect to Instapay
                        return Redirect(paymentResult.PaymentUrl);
                    }
                }
                else
                {
                    TempData["error"] = paymentResult.ErrorMessage;
                    return RedirectToAction("PaymentFailed", new { orderId = orderId });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while processing payment.";
                return RedirectToAction("PaymentFailed", new { orderId = orderId });
            }
        }




        // GET: /Checkout/ProcessWalletPayment
        [HttpGet]
        public async Task<IActionResult> ProcessWalletPayment(int orderId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var order = _unitOfWork.OrderRepository.GetOrderWithItems(orderId);

                if (order == null || order.ApplicationUserId != userId)
                {
                    TempData["error"] = "Order not found!";
                    return RedirectToAction("Index", "Home");
                }

                // معالجة الدفع عبر المحفظة
                var paymentResult = await _paymentService.ProcessWalletPaymentAsync(order);

                if (paymentResult.Success)
                {
                    // Mock wallet payment - في الإنتاج هيبقى مختلف
                    order.PaymentIntentId = paymentResult.PaymentIntentId;
                    order.PaymentStatus = "Paid";
                    order.OrderStatus = "Confirmed";
                    _unitOfWork.OrderRepository.UpdateOrder(order);
                    _unitOfWork.Save();

                    return RedirectToAction("PaymentSuccess", new { orderId = orderId });
                }
                else
                {
                    TempData["error"] = paymentResult.ErrorMessage;
                    return RedirectToAction("PaymentFailed", new { orderId = orderId });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while processing wallet payment.";
                return RedirectToAction("PaymentFailed", new { orderId = orderId });
            }
        }




        // الـ Mock Payment Page
        [HttpGet]
        public IActionResult MockPayment(int orderId, string paymentIntentId)
        {
            ViewBag.OrderId = orderId;
            ViewBag.PaymentIntentId = paymentIntentId;
            return View();
        }



        //  الـ Mock Payment Processing
        [HttpPost]
        public async Task<IActionResult> ProcessMockPayment(int orderId, string paymentIntentId, bool success = true)
        {
            if (success)
            {
                // Verify payment (في الحقيقي هيبقى عبر webhook)
                var paymentVerified = await _paymentService.VerifyInstapayPaymentAsync(paymentIntentId);

                if (paymentVerified)
                {
                    // Update order status
                    var order = _unitOfWork.OrderRepository.GetOrderById(orderId);
                    if (order != null)
                    {
                        order.PaymentStatus = "Paid";
                        order.OrderStatus = "Confirmed";
                        order.PaymentIntentId = paymentIntentId;
                        _unitOfWork.OrderRepository.UpdateOrder(order);
                        _unitOfWork.Save();
                    }

                    return RedirectToAction("PaymentSuccess", new { orderId = orderId });
                }
            }

            return RedirectToAction("PaymentFailed", new { orderId = orderId });
        }




    }
}