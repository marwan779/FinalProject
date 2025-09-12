using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;


namespace InventoryManagementSystem.DataAccess.Repository
{
    public class SaleOrderRepository: Repository<SaleOrder>, ISaleOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleOrderRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }
        public void Update(SaleOrder saleOrder)
        {
            _context.SaleOrders.Update(saleOrder);
        }
    }
}
