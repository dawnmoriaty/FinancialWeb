using FinancialWeb.Models.Entity;

namespace FinancialWeb.Services
{
    public interface IUserService
    {
        // lấy theo id
        Task<User> GetByIdAsync(int id);
        // lấy theo username
        Task<User> GetByUsernameAsync(string username);
        // lấy theo email
        Task<User> GetByEmailAsync(string email);
        // login nè
        Task<(bool Success, string Message, User User)> RegisterAsync(string username, string email, string password, string fullName);

        // Phương thức đăng nhập với đầy đủ logic
        Task<(bool Success, string Message, User User)> LoginAsync(string username, string password);

        // Phương thức đăng nhập (xử lý cookie)
        Task SignInAsync(User user, bool isPersistent = true);

        // Phương thức đăng xuất
        Task SignOutAsync();

        // Phương thức cập nhật thông tin
        Task<(bool Success, string Message)> UpdateProfileAsync(int userId, string email, string fullName);

        // Phương thức đổi mật khẩu
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
