using InventoryManagementSystem.DataAccess.Data;
using InventoryManagementSystem.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagementSystem.DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext Context)
        {
            _context = Context;
            this._dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> Filter, string? IncludeProperties = null, bool Tracked = false)
        {
            IQueryable<T> query;

            if (Tracked)
            {
                query = _dbSet;
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }

            query = query.Where(Filter);

            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var IncludePropertey in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludePropertey);
                }
            }

            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (Filter != null)
            {
                query = query.Where(Filter);
            }

            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var IncludePropertey in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(IncludePropertey);
                }
            }

            return query.ToList();


        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}
