using FinancialWeb.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinancialWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User: Username là duy nhất
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // User: Email là duy nhất
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Category - Transaction relationship
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); // Đảm bảo khớp với SQL script

            // Category - Budget relationship
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); // Đảm bảo khớp với SQL script

            // Các index khác
            modelBuilder.Entity<Transaction>().HasIndex(t => t.Date);
            modelBuilder.Entity<Transaction>().HasIndex(t => t.Type);
            modelBuilder.Entity<Budget>().HasIndex(b => b.EndDate);
        }
    }
}