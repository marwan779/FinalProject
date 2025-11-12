using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;


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



        //  Method جديدة علشان نجيب Products مع Categories
        public IEnumerable<Product> GetAllWithCategory()
        {
            return _context.Products
                .Include(p => p.Category) //  هنا بنجيب الـ Category مع كل Product
                .ToList();
        }



        public IEnumerable<Product> GetFilteredProducts(
           string? searchTerm,
           int? categoryId,
           string sortBy,
           int page,
           int pageSize,
           out int totalCount)
        {
            // نبدأ بكل الـ Products
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            //  Search: لو في search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm)
                    || p.Description.Contains(searchTerm));
            }

            //  Filter: لو اختار category معينة
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            //  Count قبل الـ Pagination
            totalCount = query.Count();

            //  Sorting
            query = sortBy.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.UnitPrice),
                "price_desc" => query.OrderByDescending(p => p.UnitPrice),
                "name" => query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            //  Pagination
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return query.ToList();
        }
    }
}
