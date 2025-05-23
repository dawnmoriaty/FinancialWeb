using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Task<int> CreateAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetByTypeAsync(string type)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUsageCountAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryExistAsync(string name, string type)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
