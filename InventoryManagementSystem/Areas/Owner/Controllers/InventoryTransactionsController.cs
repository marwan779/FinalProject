using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels.InventoryTransaction;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area(StaticDetails.OwnerRole)]
    [Authorize(Roles = StaticDetails.OwnerRole)]
    public class InventoryTransactionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<InventoryTransactionVM> transactions = new List<InventoryTransactionVM>();

            List<InventoryTransaction> inventoryTransactions = _unitOfWork.InventoryTransactionRepository
                .GetAll(IncludeProperties: "Product").ToList();

            if(inventoryTransactions.Any())
            {
                foreach (var trans in inventoryTransactions)
                {

                    transactions.Add(new InventoryTransactionVM()
                    {
                        ProductId = trans.ProductId,
                        ProductName = trans.Product.Name,
                        QuantityChanged = trans.QuantityChanged,
                        TransactionType = trans.TransactionType,
                        ReferenceId = trans.ReferenceId,
                        CreatedAt = trans.CreatedAt,
                    });
                }
            }
            
            return View(transactions);
        }
    }
}
