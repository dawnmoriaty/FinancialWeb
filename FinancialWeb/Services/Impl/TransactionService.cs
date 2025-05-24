
using FinancialWeb.ViewModels.Transaction;

namespace FinancialWeb.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        public Task<int> CreateTransactionAsync(TransactionCreateViewModel model, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTransactionAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TransactionViewModel>> GetFilteredTransactionsAsync(int userId, TransactionFilterViewModel filter)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionViewModel> GetTransactionByIdAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TransactionViewModel>> GetTransactionsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionSummaryViewModel> GetTransactionSummaryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionCreateViewModel> PrepareTransactionCreateModelAsync(int userId, string type = null)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionFilterViewModel> PrepareTransactionFilterModelAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionEditViewModel> PrepareTransactionForEditAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTransactionAsync(TransactionEditViewModel model, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
