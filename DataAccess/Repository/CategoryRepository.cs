using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }



        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }
    }
}
