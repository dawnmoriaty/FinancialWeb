using FinancialWeb.Models.Entity;
using FinancialWeb.Repositories;
using FinancialWeb.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FinancialWeb.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<(bool Success, string Message, User User)> RegisterAsync(string username, string email, string password, string fullName)
        {
            // Kiểm tra username
            if (await _userRepository.IsUsernameExistAsync(username))
                return (false, "Tên đăng nhập đã được sử dụng", null);

            // Kiểm tra email
            if (await _userRepository.IsEmailExistAsync(email))
                return (false, "Email đã được sử dụng", null);

            // Tạo mới user
            var user = new User
            {
                Username = username,
                Email = email,
                Password = PasswordHasher.HashPassword(password),
                FullName = fullName,
                Role = "user",
                IsBlocked = false,
                CreatedAt = DateTime.Now
            };
            

            user.Id = await _userRepository.CreateAsync(user);
            return (true, "Đăng ký thành công", user);
        }

        public async Task<(bool Success, string Message, User User)> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
                return (false, "Tên đăng nhập hoặc mật khẩu không đúng", null);

            if (user.IsBlocked)
                return (false, "Tài khoản của bạn đã bị khóa", null);

            if (!PasswordHasher.VerifyPassword(user.Password, password))
                return (false, "Tên đăng nhập hoặc mật khẩu không đúng", null);

            return (true, "Đăng nhập thành công", user);
        }

        public async Task SignInAsync(User user, bool isPersistent = true)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<(bool Success, string Message)> UpdateProfileAsync(int userId, string email, string fullName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "Không tìm thấy người dùng");

            // Kiểm tra nếu email thay đổi và đã tồn tại
            if (email != user.Email && await _userRepository.IsEmailExistAsync(email))
                return (false, "Email đã được sử dụng bởi tài khoản khác");

            user.Email = email;
            user.FullName = fullName;

            await _userRepository.UpdateAsync(user);
            return (true, "Cập nhật hồ sơ thành công");
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "Không tìm thấy người dùng");

            // Kiểm tra mật khẩu hiện tại
            if (!PasswordHasher.VerifyPassword(user.Password, currentPassword))
                return (false, "Mật khẩu hiện tại không đúng");

            // Cập nhật mật khẩu mới
            user.Password = PasswordHasher.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return (true, "Đổi mật khẩu thành công");
        }

        public Task<int> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUsernameExistAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailExistAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}

