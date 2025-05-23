using FinancialWeb.Models.Entity;

namespace FinancialWeb.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId);
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string type);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<(bool Success, string Message, Category Category)> CreateCategoryAsync(string name, string type, string iconPath, int userId);
        Task<(bool Success, string Message)> UpdateCategoryAsync(int id, string name, string type, string iconPath);
        Task<(bool Success, string Message)> DeleteCategoryAsync(int id);
    }
}
