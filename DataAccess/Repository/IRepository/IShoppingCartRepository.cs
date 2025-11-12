using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository
    {

        void Update(ShoppingCart cart);
        IEnumerable<ShoppingCart> GetUserCart(string userId);
        ShoppingCart GetCartItem(string userId, int productId);
        int GetCartCount(string userId);
        void ClearCart(string userId);




        // زود الـ methods دي
        void Add(ShoppingCart cart);
        void Remove(ShoppingCart cart);
        void RemoveRange(IEnumerable<ShoppingCart> entities);
        ShoppingCart Get(int id);
        IEnumerable<ShoppingCart> GetAll();
    }
}
