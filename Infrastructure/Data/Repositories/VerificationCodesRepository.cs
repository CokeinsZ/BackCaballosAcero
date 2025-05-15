using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class VerificationCodesRepository: BaseConnection, IVerificationCodesRepository
{
    public VerificationCodesRepository(string connectionString) : base(connectionString) { }

    public async Task<string?> GetCode(int userId)
    {
        await using var connection = await GetConnectionAsync();

        const string sql = "SELECT code FROM Security_Codes WHERE user_id = @UserId";

        return await connection.QuerySingleOrDefaultAsync<string>(sql, new { UserId = userId });
    }

    public async Task Add(string code, int userId)
    {
        await using var connection = await GetConnectionAsync();

        // Verificar si ya existe un codigo para este usuario
        const string existsSql = "SELECT COUNT(*) FROM Security_Codes WHERE user_id = @UserId";
        var exists = await connection.QuerySingleAsync<int>(existsSql, new { UserId = userId });

        if (exists > 0)
        {
            const string updateSql = """
                UPDATE Security_Codes
                SET code = @Code
                WHERE user_id = @UserId
            """;
            await connection.ExecuteAsync(updateSql, new { Code = code, UserId = userId });
        }
        else
        {
            const string insertSql = """
                INSERT INTO Security_Codes(user_id, code)
                VALUES(@UserId, @Code)
            """;
            await connection.ExecuteAsync(insertSql, new { UserId = userId, Code = code });
        }
    }

    public async Task Remove(int userId)
    {
        await using var connection = await GetConnectionAsync();
        
        const string deleteSql = "DELETE FROM Security_Codes WHERE user_id = @UserId";
        
        await connection.ExecuteAsync(deleteSql, new { UserId = userId });
    }
}