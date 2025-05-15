using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class RefreshTokenRepository: BaseConnection, IRefreshTokenRepository
{
    public RefreshTokenRepository(string connectionString) : base(connectionString) { }


    public async Task<string> GetRefreshToken(int userId)
    {
        await using var connection = await GetConnectionAsync();
        
        const string sql = "SELECT token FROM Refresh_Tokens WHERE user_id = @UserId";

        return await connection.QuerySingleOrDefaultAsync<string>(sql, new { UserId = userId }) ?? string.Empty;
    }

    public async Task AddRefreshToken(int userId, string refreshToken)
    {
        await using var connection = await GetConnectionAsync();

        // Verificar si ya existe un token para este usuario
        const string existsSql = "SELECT COUNT(*) FROM Refresh_Tokens WHERE user_id = @UserId";
        var exists = await connection.QuerySingleAsync<int>(existsSql, new { UserId = userId });

        if (exists > 0)
        {
            // Actualizar el token existente
            const string updateSql = """
                UPDATE Refresh_Tokens
                SET token = @Token
                WHERE user_id = @UserId;
            """;
            await connection.ExecuteAsync(updateSql, new { Token = refreshToken, UserId = userId });
        }
        else
        {
            // Insertar un nuevo token
            const string insertSql = """
                INSERT INTO Refresh_Tokens(user_id, token)
                VALUES(@UserId, @Token)
            """; 
            await connection.ExecuteAsync(insertSql, new { UserId = userId, Token = refreshToken });
        }
    }

    public async Task RemoveRefreshToken(int userId)
    {
        await using var connection = await GetConnectionAsync();

        const string deleteSql = "DELETE FROM Refresh_Tokens WHERE user_id = @UserId";
        await connection.ExecuteAsync(deleteSql, new { UserId = userId });
    }
}