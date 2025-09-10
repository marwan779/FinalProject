using InventoryManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface IProductRepository: IRepository<Product>
    {
        public interface IProductRepository : IRepository<Product>
        {
            void Update(Product product);
        }
    }
}
