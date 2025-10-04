using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            try
            {
                List<Product> products = _unitOfWork.ProductRepository.GetAll(IncludeProperties: "Category,").ToList();


                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }
    }
}
