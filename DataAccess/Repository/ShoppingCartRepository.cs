using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class ShoppingCartRepository:IShoppingCartRepository
    {

        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext Context) 
        {
            _context = Context;
        }

        public void Update(ShoppingCart cart)
        {
            _context.ShoppingCarts.Update(cart);
        }

        // جلب كل عناصر الـ Cart للـ User
        public IEnumerable<ShoppingCart> GetUserCart(string userId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Product)
                .ThenInclude(p => p.Category)
                .Where(c => c.ApplicationUserId == userId)
                .ToList();
        }

        // جلب عنصر معين من الـ Cart
        public ShoppingCart GetCartItem(string userId, int productId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Product)
                .FirstOrDefault(c => c.ApplicationUserId == userId && c.ProductId == productId);
        }

        // عدد العناصر في الـ Cart
        public int GetCartCount(string userId)
        {
            return _context.ShoppingCarts
                .Where(c => c.ApplicationUserId == userId)
                .Sum(c => c.Quantity);
        }

        // مسح الـ Cart بالكامل
        public void ClearCart(string userId)
        {
            var cartItems = _context.ShoppingCarts
                .Where(c => c.ApplicationUserId == userId);

            _context.ShoppingCarts.RemoveRange(cartItems);
        }



        // Methods from IShoppingCartRepository
        public void Add(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
        }

       

        public void Remove(ShoppingCart cart)
        {
            _context.ShoppingCarts.Remove(cart);
        }

        public void RemoveRange(IEnumerable<ShoppingCart> entities)
        {
            _context.ShoppingCarts.RemoveRange(entities);
        }

        public ShoppingCart Get(int id)
        {
            return _context.ShoppingCarts.Find(id);
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            return _context.ShoppingCarts.ToList();
        }

       
    }
}
