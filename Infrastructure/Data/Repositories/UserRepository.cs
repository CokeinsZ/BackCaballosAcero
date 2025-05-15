using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class UserRepository: BaseConnection, IUserRepository
{
    public UserRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<User>> GetAll()
    {
        await using var connection = await GetConnectionAsync();
        
        var query = "SELECT * FROM Users;";
        
        return await connection.QueryAsync<User>(query);
    }

    public async Task<User?> GetById(int id)
    {
        await using var connection = await GetConnectionAsync();

        var query = """
                    SELECT * FROM Users
                    WHERE id = @id;
                    """;
        
        return await connection.QuerySingleOrDefaultAsync<User>(query, new { id });
    }

    public async Task<User?> GetByEmail(string email)
    {
        await using var connection = await GetConnectionAsync();

        var query = """
                    SELECT * FROM Users
                    WHERE email = @email;
                    """;
        
        var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { email = email.ToLower() });
        return user;
    }

    public async Task<User> Add(CreateUserDto user)
    {
        await using var connection = await GetConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var insertQuery = """
                INSERT INTO Users
                (name, last_name, identification_document, country, city, address, password, email, phone_number)
                VALUES
                (@Name, @LastName, @IdentificationDocument, @Country, @City, @Address, @Password, @Email, @PhoneNumber)

                RETURNING *;
            """;

            var parameters = new
            {
                Name = user.name,
                LastName = user.last_name,
                IdentificationDocument = user.identification_document,
                Country = user.country,
                City = user.city,
                Address = user.address,
                Password = user.password,
                Email = user.email.ToLower(),
                PhoneNumber = user.phone_number
            };

            var createdUser = await connection.QuerySingleAsync<User>(insertQuery, parameters, transaction);
            await transaction.CommitAsync();
            return createdUser;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<User?> Update(UpdateUserDto user, int id)
    {
        await using var connection = await GetConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // Dynamically fill the non-null parameters
            var fields = new List<string>();
            var p = new DynamicParameters();
            p.Add("Id", id);

            if (user.name != null) { fields.Add("name = @Name"); p.Add("Name", user.name); }
            if (user.last_name != null) { fields.Add("last_name = @LastName"); p.Add("LastName", user.last_name); }
            if (user.identification_document != null) { fields.Add("identification_document = @IdentificationDocument"); p.Add("IdentificationDocument", user.identification_document); }
            if (user.country != null) { fields.Add("country = @Country"); p.Add("Country", user.country); }
            if (user.city != null) { fields.Add("city = @City"); p.Add("City", user.city); }
            if (user.address != null) { fields.Add("address = @Address"); p.Add("Address", user.address); }
            if (user.phone_number != null) { fields.Add("phone_number = @PhoneNumber"); p.Add("PhoneNumber", user.phone_number); }

            if (fields.Count == 0)
                return await GetById(id); // nothing to update

            var updateQuery =
                "UPDATE Users SET " + string.Join(", ", fields) +
                " WHERE id = @Id; SELECT * FROM Users WHERE id = @Id;";
            var updatedUser = await connection.QuerySingleOrDefaultAsync<User>(updateQuery, p, transaction);

            await transaction.CommitAsync();
            return updatedUser;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<bool> VerifyUser(int id)
    {
        await using var connection = await GetConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var query = """
                UPDATE Users
                SET status = 'Active'
                WHERE id = @Id;
            """;

            var affected = await connection.ExecuteAsync(query, new { Id = id }, transaction);
            await transaction.CommitAsync();
            return affected > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ChangePassword(int id, string password)
    {
        await using var connection = await GetConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var query = """
                UPDATE Users
                SET password = @Password
                WHERE id = @Id;
            """;

            var affected = await connection.ExecuteAsync(query, new { Password = password, Id = id }, transaction);
            await transaction.CommitAsync();
            return affected > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task Delete(int id)
    {
        await using var connection = await GetConnectionAsync();
        var query = "DELETE FROM Users WHERE id = @Id";
        await connection.ExecuteAsync(query, new { Id = id });
    }
}