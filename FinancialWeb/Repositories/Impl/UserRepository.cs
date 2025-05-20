using FinancialWeb.Data;
using FinancialWeb.Models.Entity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinancialWeb.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        // Tiêm Depnendency Injection cho connection string
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Username, Email, Password, FullName, AvatarPath, Role, IsBlocked, CreatedAt
                FROM Users 
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapUserFromReader(reader);
            }

            return null;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            const string sql = @"
                SELECT Id, Username, Email, Password, FullName, AvatarPath, Role, IsBlocked, CreatedAt
                FROM Users 
                WHERE Username = @Username";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapUserFromReader(reader);
            }

            return null;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT Id, Username, Email, Password, FullName, AvatarPath, Role, IsBlocked, CreatedAt
                FROM Users 
                WHERE Email = @Email";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapUserFromReader(reader);
            }

            return null;
        }

        public async Task<int> CreateAsync(User user)
        {
            const string sql = @"
                INSERT INTO Users (Username, Email, Password, FullName, AvatarPath, Role, IsBlocked, CreatedAt)
                VALUES (@Username, @Email, @Password, @FullName, @AvatarPath, @Role, @IsBlocked, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = user.Email;
            command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = user.Password;
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = (object)user.FullName ?? DBNull.Value;
            command.Parameters.Add("@AvatarPath", SqlDbType.NVarChar).Value = (object)user.AvatarPath ?? DBNull.Value;
            command.Parameters.Add("@Role", SqlDbType.NVarChar, 10).Value = user.Role;
            command.Parameters.Add("@IsBlocked", SqlDbType.Bit).Value = user.IsBlocked;
            command.Parameters.Add("@CreatedAt", SqlDbType.DateTime2).Value = user.CreatedAt;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = @"
                UPDATE Users 
                SET Username = @Username, 
                    Email = @Email, 
                    FullName = @FullName, 
                    AvatarPath = @AvatarPath, 
                    Role = @Role, 
                    IsBlocked = @IsBlocked
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = user.Id;
            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = user.Email;
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = (object)user.FullName ?? DBNull.Value;
            command.Parameters.Add("@AvatarPath", SqlDbType.NVarChar).Value = (object)user.AvatarPath ?? DBNull.Value;
            command.Parameters.Add("@Role", SqlDbType.NVarChar, 10).Value = user.Role;
            command.Parameters.Add("@IsBlocked", SqlDbType.Bit).Value = user.IsBlocked;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> IsUsernameExistAsync(string username)
        {
            const string sql = @"
                SELECT COUNT(1) FROM Users WHERE Username = @Username";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            await connection.OpenAsync();
            var count = Convert.ToInt32(await command.ExecuteScalarAsync());

            return count > 0;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            const string sql = @"
                SELECT COUNT(1) FROM Users WHERE Email = @Email";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            await connection.OpenAsync();
            var count = Convert.ToInt32(await command.ExecuteScalarAsync());

            return count > 0;
        }

        // Helper method để chuyển từ SqlDataReader sang User object , cái này có thể đọc ModelMaper bên java , có 1 thư viện maven chuyên để map 
        private User MapUserFromReader(SqlDataReader reader)
        {
            return new User
            {
                Id = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                AvatarPath = reader["AvatarPath"] != DBNull.Value ? reader["AvatarPath"].ToString() : null,
                Role = reader["Role"].ToString(),
                IsBlocked = Convert.ToBoolean(reader["IsBlocked"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
