using InventoryManagementSystem.Models.Entities;


namespace InventoryManagementSystem.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public void Update(Category category);

        IEnumerable<Category> GetAllCategories();

    }
}
