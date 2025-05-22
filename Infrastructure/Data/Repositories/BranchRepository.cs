using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class BranchRepository: BaseConnection, IBranchRepository
    
{
    public BranchRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Branch>> GetAll()
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Branches";

        return await conn.QueryAsync<Branch>(sql);
    }

    public async Task<Branch?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Branches WHERE id = @ID";

        return await conn.QuerySingleOrDefaultAsync<Branch>(sql, new {ID = id});
    }

    public async Task<Branch> Create(CreateBranchDto dto)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
                           INSERT INTO Branches 
                           (nit, name, country, city, address) 
                           VALUES 
                           (@Nit, @Name, @Country, @City, @Address) 
                           RETURNING *
                           """;
        return await conn.QuerySingleAsync<Branch>(sql, new
        {
            Nit = dto.nit,
            Name = dto.name.ToLower(),
            Country = dto.country.ToLower(),
            City = dto.city.ToLower(),
            Address = dto.address?.ToLower()
        });
    }

    public async Task<Branch?> Update(UpdateBranchDto dto, int id)
    {
        await using var conn = await GetConnectionAsync();
        
        var sb = new StringBuilder("UPDATE Branches SET ");
        var hasSet = false;

        if (dto.nit is not null)
        {
            sb.Append("nit = @Nit");
            hasSet = true;
        }
        if (dto.name is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("name = @Name");
            hasSet = true;
        }
        if (dto.country is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("country = @Country");
            hasSet = true;
        }
        if (dto.city is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("city = @City");
            hasSet = true;
        }
        if (dto.address is not null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("address = @Address");
            hasSet = true;
        }

        if (!hasSet)
            throw new Exception("No fields to update");

        sb.Append(" WHERE id = @ID RETURNING *");

        return await conn.QuerySingleOrDefaultAsync<Branch>(sb.ToString(), new
        {
            Nit = dto.nit,
            Name = dto.name?.ToLower(),
            Country = dto.country?.ToLower(),
            City = dto.city?.ToLower(),
            Address = dto.address?.ToLower(),
            ID = id
        });
    }

    public async Task<bool> Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM Branches WHERE id = @ID";
        
        return await conn.ExecuteAsync(sql, new {ID = id}) > 0;
    }
}