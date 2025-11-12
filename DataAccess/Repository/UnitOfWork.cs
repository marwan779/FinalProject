using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IInventoryTransactionRepository InventoryTransactionRepository { get; private set; }
        public IPurchaseOrderRepository PurchaseOrderRepository { get; private set; }
        public ISaleOrderRepository SaleOrderRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        private readonly ApplicationDbContext _context;


        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            ProductRepository = new ProductRepository(_context);

            CategoryRepository = new CategoryRepository(_context);

            InventoryTransactionRepository = new InventoryTransactionRepository(_context);

            PurchaseOrderRepository = new PurchaseOrderRepository(_context);

            SaleOrderRepository = new SaleOrderRepository(_context);

            ApplicationUserRepository = new ApplicationUserRepository(_context);

            ShoppingCartRepository = new ShoppingCartRepository(_context);

           OrderRepository = new OrderRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
