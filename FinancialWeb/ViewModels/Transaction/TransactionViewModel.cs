using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Số tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} VND")]
        public decimal Amount { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Ngày")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [Display(Name = "ID giao dịch")]
        public int CategoryId { get; set; }

        [Display(Name = "Danh mục")]
        public string CategoryName { get; set; }

        [Display(Name = "Loại")]
        public string CategoryType { get; set; }

        public string IconPath { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedAt { get; set; }
    }
}
