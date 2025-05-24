using FinancialWeb.ViewModels.Transaction;

namespace FinancialWeb.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionViewModel>> GetTransactionsByUserIdAsync(int userId);
        Task<TransactionViewModel> GetTransactionByIdAsync(int id, int userId);
        Task<IEnumerable<TransactionViewModel>> GetFilteredTransactionsAsync(int userId, TransactionFilterViewModel filter);
        Task<int> CreateTransactionAsync(TransactionCreateViewModel model, int userId);
        Task<TransactionEditViewModel> PrepareTransactionForEditAsync(int id, int userId);
        Task<bool> UpdateTransactionAsync(TransactionEditViewModel model, int userId);
        Task<bool> DeleteTransactionAsync(int id, int userId);
        Task<TransactionSummaryViewModel> GetTransactionSummaryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<TransactionCreateViewModel> PrepareTransactionCreateModelAsync(int userId, string type = null);
        Task<TransactionFilterViewModel> PrepareTransactionFilterModelAsync(int userId);
    }
}
