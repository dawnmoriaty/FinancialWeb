using FinancialWeb.Models.DTO;
using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories.Impl
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;
        public TransactionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            // Implementation here...
            return new List<Transaction>();
        }
        public async Task<Transaction> GetByIdAndUserIdAsync(int id, int userId)
        {
            // Implementation here...
            return new Transaction();
        }
        public async Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Implementation here...
            return new List<Transaction>();
        }
        public async Task<IEnumerable<Transaction>> GetByUserIdAndCategoryTypeAsync(int userId, string categoryType)
        {
            // Implementation here...
            return new List<Transaction>();
        }
        public async Task<decimal> GetTotalIncomeByUserIdAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Implementation here...
            return 0;
        }
        public async Task<decimal> GetTotalExpenseByUserIdAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Implementation here...
            return 0;
        }
        public async Task<IEnumerable<Transaction>> GetRecentByUserIdAsync(int userId, int count)
        {
            // Implementation here...
            return new List<Transaction>();
        }
        public async Task<int> CreateAsync(Transaction transaction)
        {
            // Implementation here...
            return 0;
        }
        public async Task<bool> UpdateAsync(Transaction transaction)
        {
            // Implementation here...
            return false;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            // Implementation here...
            return false;
        }
        public async Task<IEnumerable<CategorySummary>> GetMonthlyCategorySummaryAsync(int userId, DateTime month)
        {
            // Implementation here...
            return new List<CategorySummary>();
        }
        public async Task<IEnumerable<Transaction>> GetAllAsync() {
            throw new NotImplementedException("This method is not implemented in this repository.");
        }
        
    }
}
