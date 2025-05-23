using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Category>> GetByTypeAsync(string type);
        Task<Category> GetByIdAsync(int id);
        Task<bool> IsCategoryExistAsync(string name, string type);
        Task<int> CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<int> GetUsageCountAsync(int categoryId);
    }
}
