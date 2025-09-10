using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class InventoryTransactionRepository: Repository<InventoryTransaction>, IInventoryTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryTransactionRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }
        public void Update(InventoryTransaction inventoryTransaction)
        {
            _context.InventoryTransactions.Update(inventoryTransaction);
        }
    }
}
