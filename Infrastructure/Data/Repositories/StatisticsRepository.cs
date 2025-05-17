using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;
using Infrastructure.Data.Queries;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data.Repositories;

public class StatisticsRepository : BaseConnection, IStatisticsRepository
{
    private readonly IConfiguration _config;
    
    // Obtener connectionString desde IConfiguration
    public StatisticsRepository(IConfiguration config) 
        : base(config.GetConnectionString("DefaultConnection")) 
    {
        _config = config;
    }

    public async Task<Statistics?> GenerateStatisticsAsync(int branchId, DateTime startDate, DateTime endDate)
    {
        string totalIncomeQuery = await QueryHelper.LoadQueryAsync("TotalIncomeQuery");
        string bestSellerQuery = await QueryHelper.LoadQueryAsync("BestSellerQuery");
        string distributionQuery = await QueryHelper.LoadQueryAsync("SalesDistributionQuery");

        // Aplicar reemplazos después de cargar
        totalIncomeQuery = totalIncomeQuery
            .Replace("Bill_Motorcycle", "bill_motorcycle")
            .Replace("Motoinventory", "motoinventory");

        bestSellerQuery = bestSellerQuery
            .Replace("Motorcycles", "motorcycles");

        distributionQuery = distributionQuery
            .Replace("Branches", "branches");

        var parameters = new { branchId, startDate, endDate };

        await using var connection = await GetConnectionAsync();
        
        // Ejecutar consultas
        return new Statistics
        {
            TotalIncome = await connection.QueryFirstOrDefaultAsync<decimal>(totalIncomeQuery, parameters),
            BestSeller = await connection.QueryFirstOrDefaultAsync<string>(bestSellerQuery, parameters) ?? "Sin ventas",
            BranchId = branchId,
            CreatedAt = DateTime.UtcNow
        };
    }
}