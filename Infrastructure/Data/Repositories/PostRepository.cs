using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class PostRepository: BaseConnection, IPostRepository
{
    public PostRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Post>> GetByBranch(int branchId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Post WHERE branch_id = @BranchId";
        return await conn.QueryAsync<Post>(sql, new { BranchId = branchId });
    }

    public async Task<Post?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Post WHERE id = @Id";
        return await conn.QuerySingleOrDefaultAsync<Post>(sql, new { Id = id });
    }

    public async Task<Post> Create(CreatePostDto dto)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
            INSERT INTO Post
                (branch_id, description, available_customizations, price)
            VALUES
                (
                  @BranchId,
                  @Description,
                  CAST(@AvailableCustomizations AS JSONB),
                  CAST(@Price::text AS MONEY)
                )
            RETURNING *;
        """;
        return await conn.QuerySingleAsync<Post>(sql, new
        {
            BranchId               = dto.branch_id,
            Description            = dto.description,
            AvailableCustomizations = dto.availableCustomizations?.GetRawText(),
            Price                  = dto.price
        });
    }

    public async Task<Post?> Update(UpdatePostDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        var sb = new StringBuilder("UPDATE Post SET ");
        var hasSet = false;

        if (dto.description is not null)
        {
            sb.Append("description = @Description");
            hasSet = true;
        }

        if (dto.availableCustomizations.HasValue)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("available_customizations = CAST(@AvailableCustomizations AS JSONB)");
            hasSet = true;
        }

        if (dto.price.HasValue)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("price = CAST(@Price::text AS MONEY)");
            hasSet = true;
        }

        if (!hasSet)
            return null;

        sb.Append(" WHERE id = @Id");
        await conn.ExecuteAsync(sb.ToString(), new
        {
            Id                      = id,
            Description             = dto.description,
            AvailableCustomizations = dto.availableCustomizations?.GetRawText(),
            Price                   = dto.price
        });

        return await GetById(id);
    }

    public async Task<bool> ChangeStatus(int id, string status)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "UPDATE Post SET status = CAST(@Status AS post_status) WHERE id = @Id";
        var affected = await conn.ExecuteAsync(sql, new { Id = id, Status = status });
        return affected > 0;
    }

    public async Task<bool> Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM Post WHERE id = @Id";
        var affected = await conn.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }
}