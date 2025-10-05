using InventoryManagementSystem.DataAccess.Repository;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.PurchaseOrder;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class OwnerPurchaseOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnerPurchaseOrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatePurchaseOrder(int productId)
        {
            CreatePurchaseOrderVM createPurchaseOrderVM = new CreatePurchaseOrderVM();

            try
            {
                createPurchaseOrderVM.Suppliers = _unitOfWork.ApplicationUserRepository.GetAll()
                    .Select(u => new SelectListItem()
                    {
                        Text = u.UserName,
                        Value = u.Id,
                    }).ToList();

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

                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    SupplierId = createPurchaseOrderVM.SupplierId,
                    OrderDate = DateTime.UtcNow,
                    Status = StaticDetails.PurchaseOrderPending,
                    TotalAmount = createPurchaseOrderVM.CostPrice * createPurchaseOrderVM.Quantity,
                    PurchaseOrderItem = new PurchaseOrderItem
                    {
                        Quantity = createPurchaseOrderVM.Quantity,
                        ProductId = createPurchaseOrderVM.ProductId,
                        CostPrice = createPurchaseOrderVM.CostPrice,
                    }
                };


                _unitOfWork.PurchaseOrderRepository.Add(purchaseOrder);
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
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
    }
}
