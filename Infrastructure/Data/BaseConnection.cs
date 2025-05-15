using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data;

public abstract class BaseConnection
{
    private readonly string _connectionString;
    protected BaseConnection(string connectionString) => _connectionString = connectionString;

    protected async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}