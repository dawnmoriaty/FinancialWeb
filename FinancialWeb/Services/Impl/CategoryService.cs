using FinancialWeb.Models.Entity;
using FinancialWeb.Repositories;

namespace FinancialWeb.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task<(bool Success, string Message, Category Category)> CreateCategoryAsync(string name, string type, string iconPath, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string Message)> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string type)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, string Message)> UpdateCategoryAsync(int id, string name, string type, string iconPath)
        {
            throw new NotImplementedException();
        }
    }
}
