using System.ComponentModel.DataAnnotations;

namespace FinancialWeb.Models.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; } // Nullable

        public string? AvatarPath { get; set; } // Nullable

        [Required]
        [StringLength(10)]
        public string Role { get; set; } = "user";

        public bool IsBlocked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Budget>? Budgets { get; set; }
    }
}
