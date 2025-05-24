using FinancialWeb.Models.Entity;
using FinancialWeb.Repositories;
using FinancialWeb.ViewModels.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinancialWeb.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly ICategoryRepository _categoryRepo;

        public TransactionService(ITransactionRepository transactionRepo, ICategoryRepository categoryRepo)
        {
            _transactionRepo = transactionRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<TransactionViewModel>> GetUserTransactionsAsync(int userId)
        {
            var transactions = await _transactionRepo.GetByUserIdAsync(userId);
            return MapToViewModels(transactions);
        }

        public async Task<List<TransactionViewModel>> GetFilteredTransactionsAsync(int userId, TransactionFilterViewModel filter)
        {
            var transactions = await _transactionRepo.GetFilteredTransactionsAsync(
                userId,
                filter.StartDate,
                filter.EndDate,
                filter.Type,
                filter.CategoryId);

            return MapToViewModels(transactions);
        }

        public async Task<TransactionViewModel> GetTransactionByIdAsync(int id, int userId)
        {
            var transaction = await _transactionRepo.GetByIdAsync(id);

            if (transaction == null || transaction.UserId != userId)
                return null;

            return MapToViewModel(transaction);
        }

        public async Task<int> CreateTransactionAsync(TransactionFormViewModel model, int userId)
        {
            var transaction = new Transaction
            {
                Amount = model.Amount,
                Description = model.Description,
                Date = model.Date,
                CategoryId = model.CategoryId,
                UserId = userId
            };

            return await _transactionRepo.CreateAsync(transaction);
        }

        public async Task<bool> UpdateTransactionAsync(TransactionFormViewModel model, int userId)
        {
            var transaction = new Transaction
            {
                Id = model.Id,
                Amount = model.Amount,
                Description = model.Description,
                Date = model.Date,
                CategoryId = model.CategoryId,
                UserId = userId
            };

            return await _transactionRepo.UpdateAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id, int userId)
        {
            return await _transactionRepo.DeleteAsync(id, userId);
        }

        public async Task<TransactionFilterViewModel> PrepareFilterModelAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return new TransactionFilterViewModel
            {
                Categories = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Tất cả danh mục --" }
                }.Concat(
                    categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name + " (" + (c.Type == "income" ? "Thu" : "Chi") + ")"
                    })
                ).ToList()
            };
        }

        public async Task<TransactionFormViewModel> PrepareFormModelAsync(string type = null)
        {
            IEnumerable<Category> categories;

            if (type == "income" || type == "expense")
            {
                categories = await _categoryRepo.GetByTypeAsync(type);
            }
            else
            {
                categories = await _categoryRepo.GetAllAsync();
            }

            return new TransactionFormViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name + " (" + (c.Type == "income" ? "Thu" : "Chi") + ")"
                }).ToList()
            };
        }

        public async Task<TransactionFormViewModel> PrepareEditFormModelAsync(int id, int userId)
        {
            var transaction = await _transactionRepo.GetByIdAsync(id);

            if (transaction == null || transaction.UserId != userId)
                return null;

            var categories = await _categoryRepo.GetAllAsync();

            return new TransactionFormViewModel
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Date = transaction.Date,
                CategoryId = transaction.CategoryId,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name + " (" + (c.Type == "income" ? "Thu" : "Chi") + ")",
                    Selected = c.Id == transaction.CategoryId
                }).ToList()
            };
        }

        public async Task<TransactionSummaryViewModel> GetUserSummaryAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var end = endDate ?? DateTime.Today;

            var totalIncome = await _transactionRepo.GetTotalIncomeAsync(userId, start, end);
            var totalExpense = await _transactionRepo.GetTotalExpenseAsync(userId, start, end);

            return new TransactionSummaryViewModel
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                StartDate = start,
                EndDate = end
            };
        }

        public async Task<TransactionSummaryViewModel> GetSystemSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var end = endDate ?? DateTime.Today;

            var totalIncome = await _transactionRepo.GetSystemTotalIncomeAsync(start, end);
            var totalExpense = await _transactionRepo.GetSystemTotalExpenseAsync(start, end);

            return new TransactionSummaryViewModel
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                StartDate = start,
                EndDate = end
            };
        }

        private static List<TransactionViewModel> MapToViewModels(List<Transaction> transactions)
        {
            return transactions.Select(MapToViewModel).ToList();
        }

        private static TransactionViewModel MapToViewModel(Transaction t)
        {
            return new TransactionViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? "Không xác định",
                CategoryType = t.Category?.Type ?? "unknown",
                CreatedAt = t.CreatedAt
            };
        }
    }
}