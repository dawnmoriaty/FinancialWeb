using FinancialWeb.Services;
using FinancialWeb.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialWeb.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var viewModel = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                IconPath = c.IconPath,
                UserName = c.User?.Username
            }).ToList();

            return View(viewModel);
        }

        [Authorize(Roles = "admin")] // Chỉ admin mới có quyền thêm danh mục
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _categoryService.CreateCategoryAsync(
                    model.Name,
                    model.Type,
                    model.IconPath,
                    userId);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                IconPath = category.IconPath
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(
                    model.Id,
                    model.Name,
                    model.Type,
                    model.IconPath);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                IconPath = category.IconPath,
                UserName = category.User?.Username
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // API để lấy danh mục cho dropdown
        [HttpGet]
        public async Task<JsonResult> GetCategoriesByType(string type)
        {
            var categories = await _categoryService.GetCategoriesByTypeAsync(type);

            var result = categories.Select(c => new
            {
                id = c.Id,
                text = c.Name,
                icon = c.IconPath
            });

            return Json(result);
        }
    }
}
