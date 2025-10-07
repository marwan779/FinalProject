using InventoryManagementSystem.DataAccess.Repository;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.PurchaseOrder;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles =StaticDetails.OwnerRole)]
    public class OwnerPurchaseOrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public OwnerPurchaseOrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<PurchaseOrder> purchaseOrders = _unitOfWork.PurchaseOrderRepository
                .GetAll(IncludeProperties: "PurchaseOrderItem,Supplier").ToList();

            List<PurchaseOrderVM> purchaseOrderVMs = new List<PurchaseOrderVM>();

            foreach (var order in purchaseOrders)
            {
                var item = order.PurchaseOrderItem; // since one-to-one

                var vm = new PurchaseOrderVM
                {
                    PurchaseOrderId = order.PurchaseOrderId,
                    SupplierUserName = order.Supplier?.UserName ?? "Unknown",
                    ProductName = _unitOfWork.ProductRepository
                    .Get(p => p.ProductId == order.PurchaseOrderItem.ProductId).Name,
                    Quantity = item?.Quantity ?? 0,
                    CostPrice = item?.CostPrice ?? 0,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    OrderDate = order.OrderDate
                };

                purchaseOrderVMs.Add(vm);
            }

            return View(purchaseOrderVMs);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePurchaseOrder(int productId)
        {
            CreatePurchaseOrderVM createPurchaseOrderVM = new CreatePurchaseOrderVM();
            var allUsers = _userManager.Users.ToList();
            var suppliers = new List<ApplicationUser>();

            try
            {
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, StaticDetails.SupplierRole))
                    {
                        suppliers.Add(user);
                    }
                }

                createPurchaseOrderVM.Suppliers = suppliers
                 .Select(u => new SelectListItem
                 {
                     Text = u.UserName,
                     Value = u.Id
                 })
                 .ToList();

                createPurchaseOrderVM.CostPrice = _unitOfWork.ProductRepository.Get(p => p.ProductId == productId).CostPrice;

                return View(createPurchaseOrderVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return View(createPurchaseOrderVM);
            }
        }

        [HttpPost]
        public IActionResult CreatePurchaseOrder(CreatePurchaseOrderVM createPurchaseOrderVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    createPurchaseOrderVM.Suppliers = _unitOfWork.ApplicationUserRepository.GetAll()
                    .Select(u => new SelectListItem()
                    {
                        Text = u.UserName,
                        Value = u.Id,
                    }).ToList();

                    return View(createPurchaseOrderVM);
                }

                Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == createPurchaseOrderVM.ProductId);
                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    SupplierId = createPurchaseOrderVM.SupplierId,
                    OrderDate = DateTime.UtcNow,
                    Status = StaticDetails.PurchaseOrderPending,
                    TotalAmount = product.CostPrice * createPurchaseOrderVM.Quantity,
                    PurchaseOrderItem = new PurchaseOrderItem
                    {
                        Quantity = createPurchaseOrderVM.Quantity,
                        ProductId = createPurchaseOrderVM.ProductId,
                        CostPrice = product.CostPrice,
                    }
                };


                _unitOfWork.PurchaseOrderRepository.Add(purchaseOrder);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                createPurchaseOrderVM.Suppliers = _unitOfWork.ApplicationUserRepository.GetAll()
                    .Select(u => new SelectListItem()
                    {
                        Text = u.UserName,
                        Value = u.Id,
                    }).ToList();

                return View(createPurchaseOrderVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePurchaseOrder(int purchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrderRepository
                    .Get(p => p.PurchaseOrderId == purchaseOrderId, IncludeProperties: "PurchaseOrderItem");

                var allUsers = _userManager.Users.ToList();
                var suppliers = new List<ApplicationUser>();

                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, StaticDetails.SupplierRole))
                    {
                        suppliers.Add(user);
                    }
                }

                UpdatePurchaseOrderVM updatePurchaseOrderVM = new UpdatePurchaseOrderVM()
                {
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    SupplierId = purchaseOrder.SupplierId,
                    ProductId = purchaseOrder.PurchaseOrderItem.ProductId,
                    Quantity = purchaseOrder.PurchaseOrderItem.Quantity,
                    CostPrice = purchaseOrder.PurchaseOrderItem.CostPrice,
                    Total = purchaseOrder.TotalAmount,
                    Status = purchaseOrder.Status,

                    Suppliers =  suppliers
                     .Select(u => new SelectListItem
                     {
                         Text = u.UserName,
                         Value = u.Id
                     })
                     .ToList()
                };

                return View(updatePurchaseOrderVM);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult UpdatePurchaseOrder(UpdatePurchaseOrderVM updatePurchaseOrderVM)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    updatePurchaseOrderVM.Suppliers = _unitOfWork.ApplicationUserRepository.GetAll()
                        .Select(u => new SelectListItem
                        {
                            Text = u.UserName,
                            Value = u.Id
                        }).ToList();

                    return View(updatePurchaseOrderVM);
                }

                PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrderRepository
                    .Get(p => p.PurchaseOrderId == updatePurchaseOrderVM.PurchaseOrderId, IncludeProperties: "PurchaseOrderItem");

                Product product = _unitOfWork.ProductRepository.Get(p => p.ProductId == updatePurchaseOrderVM.ProductId);

                purchaseOrder.SupplierId = updatePurchaseOrderVM.SupplierId;
                purchaseOrder.TotalAmount = product.CostPrice * updatePurchaseOrderVM.Quantity;
                purchaseOrder.OrderDate = DateTime.UtcNow;


                purchaseOrder.PurchaseOrderItem.ProductId = updatePurchaseOrderVM.ProductId;
                purchaseOrder.PurchaseOrderItem.Quantity = updatePurchaseOrderVM.Quantity;
                purchaseOrder.PurchaseOrderItem.CostPrice = product.CostPrice;
                purchaseOrder.Status = updatePurchaseOrderVM.Status;

                if(purchaseOrder.Status == StaticDetails.PurchaseOrderDelivered)
                {
                    product.QuantityInStock = updatePurchaseOrderVM.Quantity;
                }

                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.PurchaseOrderRepository.Update(purchaseOrder);
                _unitOfWork.Save();


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return RedirectToAction(nameof(Index));
            }
        }



        [HttpGet]
        public IActionResult CancelPurchaseOrder(int purchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrderRepository.Get(p => p.PurchaseOrderId == purchaseOrderId);

                _unitOfWork.PurchaseOrderRepository.Remove(purchaseOrder);  
                
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
