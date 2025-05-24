using FinancialWeb.Models.Entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FinancialWeb.Repositories.Impl
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Transaction>> GetByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT t.Id, t.Amount, t.Description, t.Date, t.CategoryId, t.UserId, t.CreatedAt,
                       c.Name AS CategoryName, c.Type AS CategoryType, c.IconPath
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.UserId = @UserId
                ORDER BY t.Date DESC";

            var transactions = new List<Transaction>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transactions.Add(MapFromReader(reader));
            }

            return transactions;
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT t.Id, t.Amount, t.Description, t.Date, t.CategoryId, t.UserId, t.CreatedAt,
                       c.Name AS CategoryName, c.Type AS CategoryType, c.IconPath
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapFromReader(reader);
            }

            return null;
        }

        public async Task<List<Transaction>> GetFilteredTransactionsAsync(int userId, DateTime? startDate, DateTime? endDate, string type, int? categoryId)
        {
            var sql = @"
                SELECT t.Id, t.Amount, t.Description, t.Date, t.CategoryId, t.UserId, t.CreatedAt,
                       c.Name AS CategoryName, c.Type AS CategoryType, c.IconPath
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.UserId = @UserId";

            if (startDate.HasValue)
                sql += " AND t.Date >= @StartDate";

            if (endDate.HasValue)
                sql += " AND t.Date <= @EndDate";

            if (!string.IsNullOrEmpty(type))
                sql += " AND c.Type = @Type";

            if (categoryId.HasValue && categoryId.Value > 0)
                sql += " AND t.CategoryId = @CategoryId";

            sql += " ORDER BY t.Date DESC";

            var transactions = new List<Transaction>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            if (startDate.HasValue)
                command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Value.Date;

            if (endDate.HasValue)
                command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Value.Date.AddDays(1).AddSeconds(-1);

            if (!string.IsNullOrEmpty(type))
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = type;

            if (categoryId.HasValue && categoryId.Value > 0)
                command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId.Value;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transactions.Add(MapFromReader(reader));
            }

            return transactions;
        }

        public async Task<decimal> GetTotalIncomeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                SELECT COALESCE(SUM(t.Amount), 0)
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.UserId = @UserId 
                  AND t.Date >= @StartDate 
                  AND t.Date <= @EndDate
                  AND c.Type = 'income'";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Date.AddDays(1).AddSeconds(-1);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public async Task<decimal> GetTotalExpenseAsync(int userId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                SELECT COALESCE(SUM(t.Amount), 0)
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.UserId = @UserId 
                  AND t.Date >= @StartDate 
                  AND t.Date <= @EndDate
                  AND c.Type = 'expense'";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Date.AddDays(1).AddSeconds(-1);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public async Task<int> CreateAsync(Transaction transaction)
        {
            const string sql = @"
                INSERT INTO Transactions (Amount, Description, Date, CategoryId, UserId, CreatedAt)
                VALUES (@Amount, @Description, @Date, @CategoryId, @UserId, @CreatedAt);
                SELECT SCOPE_IDENTITY();";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = transaction.Amount;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = transaction.Description;
            command.Parameters.Add("@Date", SqlDbType.Date).Value = transaction.Date;
            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = transaction.CategoryId;
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = transaction.UserId;
            command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = DateTime.Now;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Transaction transaction)
        {
            const string sql = @"
                UPDATE Transactions 
                SET Amount = @Amount, 
                    Description = @Description, 
                    Date = @Date, 
                    CategoryId = @CategoryId
                WHERE Id = @Id AND UserId = @UserId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = transaction.Id;
            command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = transaction.Amount;
            command.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = transaction.Description;
            command.Parameters.Add("@Date", SqlDbType.Date).Value = transaction.Date;
            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = transaction.CategoryId;
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = transaction.UserId;

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            const string sql = "DELETE FROM Transactions WHERE Id = @Id AND UserId = @UserId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<decimal> GetSystemTotalIncomeAsync(DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                SELECT COALESCE(SUM(t.Amount), 0)
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.Date >= @StartDate 
                  AND t.Date <= @EndDate
                  AND c.Type = 'income'";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Date.AddDays(1).AddSeconds(-1);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public async Task<decimal> GetSystemTotalExpenseAsync(DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                SELECT COALESCE(SUM(t.Amount), 0)
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryId = c.Id
                WHERE t.Date >= @StartDate 
                  AND t.Date <= @EndDate
                  AND c.Type = 'expense'";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate.Date;
            command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate.Date.AddDays(1).AddSeconds(-1);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public async Task<int> GetTransactionCountAsync()
        {
            const string sql = "SELECT COUNT(*) FROM Transactions";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        private static Transaction MapFromReader(SqlDataReader reader)
        {
            return new Transaction
            {
                Id = Convert.ToInt32(reader["Id"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Description = reader["Description"].ToString(),
                Date = Convert.ToDateTime(reader["Date"]),
                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                Category = new Category
                {
                    Id = Convert.ToInt32(reader["CategoryId"]),
                    Name = reader["CategoryName"].ToString(),
                    Type = reader["CategoryType"].ToString(),
                    IconPath = reader["IconPath"] != DBNull.Value ? reader["IconPath"].ToString() : null
                }
            };
        }
    }
}