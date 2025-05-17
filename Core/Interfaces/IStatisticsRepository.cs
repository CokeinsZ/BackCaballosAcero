using Core.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Core.Interfaces;

public class StatisticsRepository(IConfiguration config)
{
    private readonly string? _connString = config.GetConnectionString("DefaultConnection");

    public async Task<Statistics?> GetStatistics(int branchId)
    {
        const string query = $@"
            SELECT 
                SUM(bm.total) as total_income,
                CONCAT(m.brand, ' ', m.model) as best_seller,
                string_agg(s.name, ', ') as selling_distribution,
                NOW() as created_at,
                @BranchId as branch_id
            FROM bill_motorcycle bm
            JOIN motoinventory mi ON bm.inventory_moto_id = mi.id
            JOIN motorcycles m ON mi.moto_id = m.id
            JOIN branches s ON mi.branch_id = s.id
            WHERE s.id = @BranchId
            GROUP BY m.brand, m.model
            ORDER BY COUNT(bm.id) DESC
            LIMIT 1";

        await using var conn = new NpgsqlConnection(_connString);
        return await conn.QueryFirstOrDefaultAsync<Statistics>(query, new { BranchId = branchId });
    }
}