using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionFilterViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Từ ngày")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Đến ngày")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Loại")]
        public string CategoryType { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
