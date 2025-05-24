using FinancialWeb.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionSummaryViewModel
    {
        [Display(Name = "Tổng thu")]
        [DisplayFormat(DataFormatString = "{0:N0} VND")]
        public decimal TotalIncome { get; set; }

        [Display(Name = "Tổng chi")]
        [DisplayFormat(DataFormatString = "{0:N0} VND")]
        public decimal TotalExpense { get; set; }

        [Display(Name = "Số dư")]
        [DisplayFormat(DataFormatString = "{0:N0} VND")]
        public decimal Balance { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<TransactionViewModel> RecentTransactions { get; set; }
        public IEnumerable<CategorySummary> IncomeSummaries { get; set; }
        public IEnumerable<CategorySummary> ExpenseSummaries { get; set; }
    }
}
