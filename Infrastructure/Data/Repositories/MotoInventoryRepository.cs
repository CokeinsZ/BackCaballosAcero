using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class MotoInventoryRepository : BaseConnection, IMotoInventoryRepository
{
    public MotoInventoryRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<MotoInventory>> GetByBranch(int branchId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM MotoInventory WHERE branch_id = @BranchId";
        return await conn.QueryAsync<MotoInventory>(sql, new { BranchId = branchId });
    }

    public async Task<MotoInventory?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM MotoInventory WHERE id = @Id";
        return await conn.QuerySingleOrDefaultAsync<MotoInventory>(sql, new { Id = id });
    }

    public async Task<MotoInventory> Create(CreateMotoInventoryDto dto)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
            INSERT INTO MotoInventory
                (moto_id, branch_id, post_id, license_plate, km, customizations)
            VALUES
                (@MotoId, @BranchId, @PostId, @LicensePlate, @Km, CAST(@Customizations AS JSONB))
            RETURNING *;
        """;
        return await conn.QuerySingleAsync<MotoInventory>(sql, new
        {
            MotoId         = dto.moto_id,
            BranchId       = dto.branch_id,
            PostId         = dto.post_id,
            LicensePlate   = dto.license_plate,
            Km             = dto.km,
            Customizations = dto.customizations?.GetRawText()
        });
    }

    public async Task<MotoInventory?> Update(UpdateMotoInventoryDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        var sb = new StringBuilder("UPDATE MotoInventory SET ");
        var hasSet = false;

        if (dto.post_id is not null)
        {
            sb.Append("post_id = @PostId");
            hasSet = true;
        }
        if (dto.license_plate is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("license_plate = @LicensePlate");
            hasSet = true;
        }
        if (dto.km is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("km = @Km");
            hasSet = true;
        }
        if (dto.customizations is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("customizations = CAST(@Customizations AS JSONB)");
            hasSet = true;
        }

        if (!hasSet) throw new ArgumentException("No fields to update");
        sb.Append(" WHERE id = @Id");

        await conn.ExecuteAsync(sb.ToString(), new
        {
            Id             = id,
            PostId         = dto.post_id,
            LicensePlate   = dto.license_plate,
            Km             = dto.km,
            Customizations = dto.customizations?.GetRawText()
        });

        return await GetById(id);
    }

    public async Task<bool> ChangeStatus(int id, string status)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "UPDATE MotoInventory SET status = CAST(@Status AS motoInventory_status) WHERE id = @Id";
        var affected = await conn.ExecuteAsync(sql, new { Id = id, Status = status });
        return affected > 0;
    }

    public async Task<bool> Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM MotoInventory WHERE id = @Id";
        var affected = await conn.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }
}