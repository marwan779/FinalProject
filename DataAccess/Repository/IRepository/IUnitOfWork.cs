﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IInventoryTransactionRepository InventoryTransactionRepository { get; }
        public IPurchaseOrderRepository PurchaseOrderRepository  { get; }
        public ISaleOrderRepository SaleOrderRepository  { get; }
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public void Save();

    }
}
