using FinancialWeb.Models.DTO;
using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories
{
    public interface ITransactionRepository
    {
        // Lấy dữ liệu
        Task<List<Transaction>> GetByUserIdAsync(int userId);
        Task<Transaction> GetByIdAsync(int id);
        Task<List<Transaction>> GetFilteredTransactionsAsync(int userId, DateTime? startDate, DateTime? endDate, string type, int? categoryId);

        // Tính tổng
        Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpenseAsync(int userId, DateTime startDate, DateTime endDate);

        // CRUD
        Task<int> CreateAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id, int userId);

        // Cho admin
        Task<decimal> GetSystemTotalIncomeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetSystemTotalExpenseAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTransactionCountAsync();
    }
}
