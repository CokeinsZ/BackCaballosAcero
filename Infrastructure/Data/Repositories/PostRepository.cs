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
            INSERT INTO Post (branch_id, price)
            VALUES (@BranchId, @Price)
            RETURNING *;
        """;
        return await conn.QuerySingleAsync<Post>(sql, new
        {
            BranchId = dto.branch_id,
            Price    = dto.price
        });
    }

    public async Task<Post?> Update(UpdatePostDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        
        if (!dto.price.HasValue) return null;

        const string sql = "UPDATE Post SET price = @Price WHERE id = @Id";
        
        await conn.ExecuteAsync(sql, new { Id = id, Price = dto.price });

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