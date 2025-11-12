using InventoryManagementSystem.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Order/History
        public IActionResult History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _unitOfWork.OrderRepository.GetUserOrders(userId);

            return View(orders);
        }





        // GET: /Order/Details/5
        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.OrderRepository.GetOrderWithItems(id);

            if (order == null || order.ApplicationUserId != userId)
            {
                TempData["error"] = "Order not found!";
                return RedirectToAction("History");
            }

            return View(order);
        }




        // POST: /Order/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var order = _unitOfWork.OrderRepository.GetOrderById(id);

                if (order == null || order.ApplicationUserId != userId)
                {
                    TempData["error"] = "Order not found!";
                    return RedirectToAction("History");
                }

                // ممكن نلغي فقط لو مدفوعش أو لسه pending
                if (order.PaymentStatus == "Paid" && order.OrderStatus != "Pending")
                {
                    TempData["error"] = "Cannot cancel order after payment confirmation.";
                    return RedirectToAction("Details", new { id = id });
                }

                _unitOfWork.OrderRepository.CancelOrder(id);
                _unitOfWork.Save();

                TempData["success"] = "Order cancelled successfully!";
                return RedirectToAction("History");
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while cancelling the order.";
                return RedirectToAction("Details", new { id = id });
            }
        }


    }
}