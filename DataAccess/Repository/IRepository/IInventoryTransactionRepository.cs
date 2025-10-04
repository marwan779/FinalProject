using InventoryManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface IInventoryTransactionRepository: IRepository<InventoryTransaction>
    {
        void Update(InventoryTransaction transaction);
    }
}
