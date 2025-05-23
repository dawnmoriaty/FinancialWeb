using FinancialWeb.Services;
using FinancialWeb.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UpdateProfile()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var userId = int.Parse(User.FindFirst("UserId")?.Value);
            //    var result = await _userService.UpdateProfileAsync(
            //        userId,
            //        model.Email,
            //        model.FullName);

            //    if (result.Success)
            //    {
            //        TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công.";
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, result.Message);
            //    }
            //}

            return View(model);
        }
    }
}
