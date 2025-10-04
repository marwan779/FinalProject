using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
