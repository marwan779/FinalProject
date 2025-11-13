using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Areas.Supplier.Controllers
{
    [Area(StaticDetails.SupplierRole)]
    public class SupplierPurchaseOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierPurchaseOrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {


            return View();
        }
    }
}
