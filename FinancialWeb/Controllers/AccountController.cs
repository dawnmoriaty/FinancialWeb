using FinancialWeb.Services;
using FinancialWeb.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Gọi service để xử lý logic nghiệp vụ đăng ký
                var result = await _userService.RegisterAsync(
                    model.Username,
                    model.Email,
                    model.Password,
                    model.FullName);

                if (result.Success)
                {
                    // Đăng nhập người dùng sau khi đăng ký
                    await _userService.SignInAsync(result.User);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Gọi service để xử lý logic nghiệp vụ đăng nhập
                var result = await _userService.LoginAsync(model.Username, model.Password);

                if (result.Success)
                {
                    await _userService.SignInAsync(result.User, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, result.Message);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var result = await _userService.ChangePasswordAsync(
                    userId,
                    model.CurrentPassword,
                    model.NewPassword);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile() // Đổi tên từ UpdateProfile thành Profile
        {
            // Lấy userId từ ClaimTypes.NameIdentifier thay vì "UserId"
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Home");
            }

            var model = new UpdateProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName ?? string.Empty // Fix null reference warning
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UpdateProfileViewModel model) // Đổi tên từ UpdateProfile thành Profile
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lấy userId từ ClaimTypes.NameIdentifier thay vì "UserId"
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _userService.UpdateProfileAsync(
                userId,
                model.Email,
                model.FullName ?? string.Empty);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công.";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
    }
}
