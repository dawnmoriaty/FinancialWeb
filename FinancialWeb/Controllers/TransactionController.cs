using FinancialWeb.Services;
using FinancialWeb.ViewModels.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinancialWeb.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // Lấy ID người dùng từ Claims
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        // Xem danh sách giao dịch
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();

            // Lấy filter và giao dịch
            var filter = await _transactionService.PrepareFilterModelAsync();
            var transactions = await _transactionService.GetUserTransactionsAsync(userId);
            var summary = await _transactionService.GetUserSummaryAsync(userId);

            ViewBag.Filter = filter;
            ViewBag.Summary = summary;

            return View(transactions);
        }

        // Lọc giao dịch
        [HttpPost]
        public async Task<IActionResult> Index(TransactionFilterViewModel filter)
        {
            int currentUserId = GetUserId(); // Renamed variable to avoid conflict

            // Lọc giao dịch theo filter
            var transactions = await _transactionService.GetFilteredTransactionsAsync(currentUserId, filter);
            var summary = await _transactionService.GetUserSummaryAsync(currentUserId, filter.StartDate, filter.EndDate);

            // Điền lại danh sách Category
            filter = await _transactionService.PrepareFilterModelAsync();

            ViewBag.Filter = filter;
            ViewBag.Summary = summary;

            return View(transactions);
        }

        // Tạo giao dịch mới - chỉ user
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create(string type)
        {
            var model = await _transactionService.PrepareFormModelAsync(type);
            ViewBag.Type = type;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _transactionService.PrepareFormModelAsync();
                return View(model);
            }

            int userId = GetUserId();
            await _transactionService.CreateTransactionAsync(model, userId);

            TempData["Message"] = "Thêm giao dịch thành công";
            return RedirectToAction("Index");
        }

        // Sửa giao dịch - chỉ user
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(int id)
        {
            int userId = GetUserId();
            var model = await _transactionService.PrepareEditFormModelAsync(id, userId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                int currentUserId = GetUserId(); // Renamed variable to avoid conflict
                model = await _transactionService.PrepareEditFormModelAsync(model.Id, currentUserId);
                return View(model);
            }

            int userId = GetUserId();
            var success = await _transactionService.UpdateTransactionAsync(model, userId);

            if (success)
            {
                TempData["Message"] = "Cập nhật giao dịch thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Không thể cập nhật giao dịch";
                return RedirectToAction("Index");
            }
        }

        // Xóa giao dịch - chỉ user
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetUserId();
            var transaction = await _transactionService.GetTransactionByIdAsync(id, userId);

            if (transaction == null)
                return NotFound();

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = GetUserId();
            var success = await _transactionService.DeleteTransactionAsync(id, userId);

            if (success)
                TempData["Message"] = "Xóa giao dịch thành công";
            else
                TempData["Error"] = "Không thể xóa giao dịch";

            return RedirectToAction("Index");
        }

        // Báo cáo tổng quan - dành cho cả user và admin
        public async Task<IActionResult> Dashboard()
        {
            if (User.IsInRole("admin"))
            {
                // Admin xem tổng hệ thống
                var summary = await _transactionService.GetSystemSummaryAsync();
                return View("AdminDashboard", summary);
            }
            else
            {
                // User xem của riêng họ
                int userId = GetUserId();
                var summary = await _transactionService.GetUserSummaryAsync(userId);
                return View(summary);
            }
        }
    }
}