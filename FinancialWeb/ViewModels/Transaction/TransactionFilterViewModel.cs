using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionFilterViewModel
    {
        [Display(Name = "Từ ngày")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } = DateTime.Today.AddMonths(-1);

        [Display(Name = "Đến ngày")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } = DateTime.Today;

        [Display(Name = "Loại")]
        public string Type { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
