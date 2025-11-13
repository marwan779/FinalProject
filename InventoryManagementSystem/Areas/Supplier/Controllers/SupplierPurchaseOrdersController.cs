using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.PurchaseOrder;
using InventoryManagementSystem.Models.ViewModels.Supplier;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Areas.Supplier.Controllers
{
    [Authorize(Roles = StaticDetails.SupplierRole)]
    [Area(StaticDetails.SupplierRole)]
    public class SupplierPurchaseOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public SupplierPurchaseOrdersController(IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var supplierId = _userManager.GetUserId(User);

            if(supplierId == null)
            {
                return RedirectToAction
                        (
                            actionName: "Index",
                            controllerName: "Home",
                            routeValues: new { area = "" }
                        );
            }

            List<PurchaseOrder> purchaseOrders = _unitOfWork.PurchaseOrderRepository
                .GetAll(o => o.SupplierId == supplierId, IncludeProperties: "PurchaseOrderItem").ToList();

            List<SupplierPurchaseOrderVM> supplierPurchaseOrderVMs = new List<SupplierPurchaseOrderVM>();

            if (purchaseOrders.Any())
            {
                foreach(var purchaseOrder in purchaseOrders)
                {
                    Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == purchaseOrder.PurchaseOrderItem.ProductId);

                    supplierPurchaseOrderVMs.Add(new SupplierPurchaseOrderVM()
                    {
                        PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                        ProductName = product.Name,
                        Quantity = purchaseOrder.PurchaseOrderItem.Quantity,
                        CostPrice = purchaseOrder.PurchaseOrderItem.CostPrice,
                        TotalAmount = purchaseOrder.TotalAmount,
                        Status = purchaseOrder.Status,
                        OrderDate = purchaseOrder.OrderDate,
                    });
                }
            }

            return View(supplierPurchaseOrderVMs);
        }

        [HttpGet]
        public IActionResult UpdatePurchaseOrder(int purchaseOrderId)
        {
            PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrderRepository
                    .Get(p => p.PurchaseOrderId == purchaseOrderId, IncludeProperties: "PurchaseOrderItem");

            SupplierUpdatePurchaseOrderVM supplierUpdatePurchaseOrderVM = new SupplierUpdatePurchaseOrderVM()
            { 
                PurchaseOrderId = purchaseOrderId,
                CurrentStatus = purchaseOrder.Status,
            };

            return View(supplierUpdatePurchaseOrderVM);
        }


        [HttpPost]
        public IActionResult UpdatePurchaseOrder(SupplierUpdatePurchaseOrderVM supplierUpdatePurchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrderRepository
                    .Get(p => p.PurchaseOrderId == supplierUpdatePurchaseOrderVM.PurchaseOrderId);

            purchaseOrder.Status = supplierUpdatePurchaseOrderVM.CurrentStatus;

            _unitOfWork.PurchaseOrderRepository .Update(purchaseOrder);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));  
        }

    }
}
