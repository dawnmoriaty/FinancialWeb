using FinancialWeb.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionSummaryViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
