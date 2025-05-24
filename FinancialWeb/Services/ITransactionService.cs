using FinancialWeb.ViewModels.Transaction;

namespace FinancialWeb.Services
{
    public interface ITransactionService
    {
        // Các phương thức lấy dữ liệu
        Task<List<TransactionViewModel>> GetUserTransactionsAsync(int userId);
        Task<List<TransactionViewModel>> GetFilteredTransactionsAsync(int userId, TransactionFilterViewModel filter);
        Task<TransactionViewModel> GetTransactionByIdAsync(int id, int userId);

        // Phương thức CRUD
        Task<int> CreateTransactionAsync(TransactionFormViewModel model, int userId);
        Task<bool> UpdateTransactionAsync(TransactionFormViewModel model, int userId);
        Task<bool> DeleteTransactionAsync(int id, int userId);

        // Các phương thức để chuẩn bị form
        Task<TransactionFilterViewModel> PrepareFilterModelAsync();
        Task<TransactionFormViewModel> PrepareFormModelAsync(string type = null);
        Task<TransactionFormViewModel> PrepareEditFormModelAsync(int id, int userId);

        // Báo cáo
        Task<TransactionSummaryViewModel> GetUserSummaryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<TransactionSummaryViewModel> GetSystemSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}

