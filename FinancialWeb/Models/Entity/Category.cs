using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.Models.Entity
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; } // "income" hoặc "expense"

        public string? IconPath { get; set; } // Nullable

        [Required]
        public int UserId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<Budget>? Budgets { get; set; }
    }
}
