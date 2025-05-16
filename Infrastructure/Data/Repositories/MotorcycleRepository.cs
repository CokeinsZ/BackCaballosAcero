using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class MotorcycleRepository: BaseConnection, IMotorcycleRepository
    
{
    public MotorcycleRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<IEnumerable<Motorcycle>> GetAll()
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Motorcycles";
        return await conn.QueryAsync<Motorcycle>(sql);
    }

    public async Task<Motorcycle?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Motorcycles WHERE id = @id";
        return await conn.QuerySingleOrDefaultAsync<Motorcycle>(sql, new { id });
    }

    public async Task<IEnumerable<Motorcycle>> GetByFilters(string? brand, string? model, string? cc, string? color)
    {
        await using var conn = await GetConnectionAsync();
        var sql = new StringBuilder("SELECT * FROM Motorcycles WHERE 1=1");
        if (!string.IsNullOrEmpty(brand))
            sql.Append(" AND brand ILIKE @Brand");
        if (!string.IsNullOrEmpty(model))
            sql.Append(" AND model ILIKE @Model");
        if (!string.IsNullOrEmpty(cc))
            sql.Append(" AND cc ILIKE @CC");
        if (!string.IsNullOrEmpty(color))
            sql.Append(" AND color ILIKE @Color");

        return await conn.QueryAsync<Motorcycle>(sql.ToString(), new
        {
            Brand = $"%{brand?.ToLower()}%",
            Model = $"%{model?.ToLower()}%",
            CC    = $"%{cc}%",
            Color = $"%{color?.ToLower()}%"
        });
    }

    public async Task<Motorcycle> Create(CreateMotorcycleDto dto)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = $"""
            INSERT INTO Motorcycles
                (brand, model, cc, color, details)
            VALUES
                (@Brand, @Model, @CC, @Color, CAST(@Details AS JSONB))
                
            RETURNING *;
        """;
        var detailsJson = dto.Details?.GetRawText();
        return await conn.QuerySingleAsync<Motorcycle>(sql, new
            {
                Brand = dto.Brand.ToLower(),
                Model = dto.Model.ToLower(),
                dto.CC,
                Color = dto.Color?.ToLower(),
                Details = detailsJson,
            }
        );
    }

    public async Task Update(UpdateMotorcycleDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        var sb = new StringBuilder("UPDATE Motorcycles SET updated_at = @UpdatedAt");
        if (dto.Brand is not null)   sb.Append(", brand = @Brand");
        if (dto.Model is not null)   sb.Append(", model = @Model");
        if (dto.CC is not null)      sb.Append(", cc = @CC");
        if (dto.Color is not null)   sb.Append(", color = @Color");
        if (dto.Details.HasValue)     sb.Append(", details = CAST(@Details AS JSONB)");
        sb.Append(" WHERE id = @Id");
        var now = DateTime.UtcNow;
        var detailsJson = dto.Details?.GetRawText();
        await conn.ExecuteAsync(sb.ToString(), new
            {
                Id = id,
                Brand = dto.Brand?.ToLower(),
                Model = dto.Model?.ToLower(),
                dto.CC,
                Color = dto.Color?.ToLower(),
                Details = detailsJson,
                UpdatedAt = now
            }
        );
    }

    public async Task Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM Motorcycles WHERE id = @id";
        await conn.ExecuteAsync(sql, new { id });
    }
}