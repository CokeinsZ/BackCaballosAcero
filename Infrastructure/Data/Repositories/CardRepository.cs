using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class CardRepository: BaseConnection, ICardRepository
{
    public CardRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Card>> GetByUserId(int userId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Card WHERE user_id = @UserId";
        return await conn.QueryAsync<Card>(sql, new { UserId = userId });
    }

    public async Task<Card?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Card WHERE id = @Id";
        return await conn.QuerySingleOrDefaultAsync<Card>(sql, new { Id = id });
    }

    public async Task<Card> Create(CreateCardDto dto)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = $"""
            INSERT INTO Card
                (user_id, owner, pan, cvv, type, expiration_date)
            VALUES
                (@UserId, @Owner, @Pan, @Cvv, CAST(@Type as card_type), @ExpirationDate)
            RETURNING *;
        """;

        return await conn.QuerySingleAsync<Card>(sql, new
        {
            UserId = dto.user_id,
            Owner = dto.owner,
            Pan   = dto.pan,
            Cvv   = dto.cvv,
            Type  = dto.type,
            ExpirationDate = dto.expiration_date
        });
    }

    public async Task Update(UpdateCardDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        var sb = new StringBuilder("UPDATE Card SET ");
        var hasSet = false;

        if (dto.owner is not null)
        {
            sb.Append("owner = @Owner");
            hasSet = true;
        }
        if (dto.pan is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("pan = @Pan");
            hasSet = true;
        }
        if (dto.cvv is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("cvv = @Cvv");
            hasSet = true;
        }
        if (dto.type is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("type = CAST(@Type as card_type)");
            hasSet = true;
        }
        if (dto.expiration_date is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("expiration_date = @ExpirationDate");
            hasSet = true;
        }
        
        if (!hasSet) throw new ArgumentException("No fields to update");

        sb.Append(" WHERE id = @Id");

        await conn.ExecuteAsync(sb.ToString(), new
        {
            Id = id,
            Owner = dto.owner,
            Pan   = dto.pan,
            Cvv   = dto.cvv,
            Type  = dto.type,
            ExpirationDate = dto.expiration_date
        });
    }

    public async Task ChangeCardStatus(int id, string status)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "UPDATE Card SET status = CAST(@Status AS card_status) WHERE id = @Id";
        await conn.ExecuteAsync(sql, new { Id = id, Status = status });
    }

    public async Task Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM Card WHERE id = @Id";
        await conn.ExecuteAsync(sql, new { Id = id });
    }
}