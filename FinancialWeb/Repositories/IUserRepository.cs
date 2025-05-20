using FinancialWeb.Models.Entity;

namespace FinancialWeb.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<int> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IsUsernameExistAsync(string username);
        Task<bool> IsEmailExistAsync(string email);
    }
}
