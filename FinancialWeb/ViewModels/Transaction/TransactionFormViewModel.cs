using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.ViewModels.Transaction
{
    public class TransactionFormViewModel
    {
        public int Id { get; set; } // Chỉ dùng cho sửa

        [Required(ErrorMessage = "Vui lòng nhập số tiền")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
