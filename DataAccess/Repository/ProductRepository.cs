using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;


namespace InventoryManagementSystem.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
