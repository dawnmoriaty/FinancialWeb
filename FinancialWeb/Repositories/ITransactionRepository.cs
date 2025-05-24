using FinancialWeb.Models.DTO;
using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories
{
    public interface ITransactionRepository
    {
        // User methods - tất cả đều dựa trên userId để đảm bảo mỗi user chỉ thấy dữ liệu của mình
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
        Task<Transaction> GetByIdAndUserIdAsync(int id, int userId); // Đảm bảo chỉ lấy giao dịch của user hiện tại
        Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetByUserIdAndCategoryTypeAsync(int userId, string categoryType);
        Task<decimal> GetTotalIncomeByUserIdAsync(int userId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalExpenseByUserIdAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetRecentByUserIdAsync(int userId, int count);
        Task<int> CreateAsync(Transaction transaction); // Kiểm tra UserId trong triển khai
        Task<bool> UpdateAsync(Transaction transaction); // Kiểm tra UserId trong triển khai
        Task<bool> DeleteAsync(int id, int userId); // Đảm bảo chỉ xóa được giao dịch của user hiện tại
        Task<IEnumerable<CategorySummary>> GetMonthlyCategorySummaryAsync(int userId, DateTime month);

        // Admin methods - nếu cần thiết (ví dụ: xem báo cáo toàn hệ thống)
        Task<IEnumerable<Transaction>> GetAllAsync(); // Chỉ admin mới dùng
    }
}
