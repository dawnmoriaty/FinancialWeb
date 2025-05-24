using FinancialWeb.Models.Entity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinancialWeb.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            const string sql = @"
                SELECT c.Id, c.Name, c.Type, c.IconPath, c.UserId, 
                       u.Username as UserName
                FROM Categories c
                INNER JOIN Users u ON c.UserId = u.Id
                ORDER BY c.Type, c.Name";

            var categories = new List<Category>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(MapCategoryFromReader(reader));
            }

            return categories;
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT c.Id, c.Name, c.Type, c.IconPath, c.UserId, 
                       u.Username as UserName
                FROM Categories c
                INNER JOIN Users u ON c.UserId = u.Id
                WHERE c.UserId = @UserId
                ORDER BY c.Type, c.Name";

            var categories = new List<Category>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(MapCategoryFromReader(reader));
            }

            return categories;
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(string type)
        {
            const string sql = @"
                SELECT c.Id, c.Name, c.Type, c.IconPath, c.UserId, 
                       u.Username as UserName
                FROM Categories c
                INNER JOIN Users u ON c.UserId = u.Id
                WHERE c.Type = @Type
                ORDER BY c.Name";

            var categories = new List<Category>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = type;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(MapCategoryFromReader(reader));
            }

            return categories;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT c.Id, c.Name, c.Type, c.IconPath, c.UserId, 
                       u.Username as UserName
                FROM Categories c
                INNER JOIN Users u ON c.UserId = u.Id
                WHERE c.Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCategoryFromReader(reader);
            }

            return null;
        }

        public async Task<bool> IsCategoryExistAsync(string name, string type)
        {
            const string sql = @"
                SELECT COUNT(1) FROM Categories 
                WHERE Name = @Name AND Type = @Type";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name;
            command.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = type;

            await connection.OpenAsync();
            var count = Convert.ToInt32(await command.ExecuteScalarAsync());

            return count > 0;
        }

        public async Task<int> CreateAsync(Category category)
        {
            const string sql = @"
                INSERT INTO Categories (Name, Type, IconPath, UserId)
                VALUES (@Name, @Type, @IconPath, @UserId);
                SELECT SCOPE_IDENTITY();";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = category.Name;
            command.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = category.Type;
            command.Parameters.Add("@IconPath", SqlDbType.NVarChar).Value = (object)category.IconPath ?? DBNull.Value;
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = category.UserId;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Category category)
        {
            const string sql = @"
                UPDATE Categories 
                SET Name = @Name, 
                    Type = @Type, 
                    IconPath = @IconPath
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = category.Id;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = category.Name;
            command.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = category.Type;
            command.Parameters.Add("@IconPath", SqlDbType.NVarChar).Value = (object)category.IconPath ?? DBNull.Value;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Categories WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> GetUsageCountAsync(int categoryId)
        {
            // Kiểm tra xem danh mục đang được sử dụng trong bao nhiêu giao dịch và ngân sách
            const string sql = @"
                SELECT 
                    (SELECT COUNT(1) FROM Transactions WHERE CategoryId = @CategoryId) +
                    (SELECT COUNT(1) FROM Budgets WHERE CategoryId = @CategoryId)";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;

            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        // Helper method để chuyển từ SqlDataReader sang Category object
        private Category MapCategoryFromReader(SqlDataReader reader)
        {
            return new Category
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString(),
                Type = reader["Type"].ToString(),
                IconPath = reader["IconPath"] != DBNull.Value ? reader["IconPath"].ToString() : null,
                UserId = Convert.ToInt32(reader["UserId"]),
                User = new User
                {
                    Id = Convert.ToInt32(reader["UserId"]),
                    Username = reader["UserName"].ToString()
                }
            };
        }
    }
}
