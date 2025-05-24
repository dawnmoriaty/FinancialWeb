using FinancialWeb.Models.Entity;
using FinancialWeb.Repositories;

namespace FinancialWeb.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId)
        {
            return await _categoryRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string type)
        {
            return await _categoryRepository.GetByTypeAsync(type);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Category Category)> CreateCategoryAsync(
            string name, string type, string iconPath, int userId)
        {
            // Kiểm tra tên danh mục đã tồn tại chưa
            if (await _categoryRepository.IsCategoryExistAsync(name, type))
            {
                return (false, "Danh mục với tên và loại này đã tồn tại", null);
            }

            var category = new Category
            {
                Name = name,
                Type = type,
                IconPath = iconPath,
                UserId = userId
            };

            category.Id = await _categoryRepository.CreateAsync(category);
            return (true, "Tạo danh mục thành công", category);
        }

        public async Task<(bool Success, string Message)> UpdateCategoryAsync(
            int id, string name, string type, string iconPath)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return (false, "Không tìm thấy danh mục");
            }

            // Kiểm tra xem tên mới (nếu đã thay đổi) đã tồn tại chưa
            if (name != category.Name && await _categoryRepository.IsCategoryExistAsync(name, type))
            {
                return (false, "Danh mục với tên và loại này đã tồn tại");
            }

            category.Name = name;
            category.Type = type;
            category.IconPath = iconPath;

            await _categoryRepository.UpdateAsync(category);
            return (true, "Cập nhật danh mục thành công");
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return (false, "Không tìm thấy danh mục");
            }

            // Kiểm tra xem danh mục có đang được sử dụng không
            int usageCount = await _categoryRepository.GetUsageCountAsync(id);
            if (usageCount > 0)
            {
                return (false, $"Không thể xóa danh mục đang được sử dụng trong {usageCount} giao dịch/ngân sách");
            }

            await _categoryRepository.DeleteAsync(id);
            return (true, "Xóa danh mục thành công");
        }
    }
}
