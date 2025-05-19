using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class BillRepository : BaseConnection, IBillRepository
{
    public BillRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Bill>> GetByBranch(int branchId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
                           SELECT b.* FROM Bill b
                           JOIN Bill_Motorcycle bm ON bm.bill_id = b.id
                           JOIN MotoInventory mi ON mi.id = bm.inventory_moto_id
                           JOIN Branches br ON br.id = mi.branch_id
                           WHERE br.id = @BranchId;
                           """;
        return await conn.QueryAsync<Bill>(sql, new { BranchId = branchId });
    }

    public async Task<IEnumerable<Bill>> GetByUser(int userId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Bill WHERE user_id = @UserId";
        return await conn.QueryAsync<Bill>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<Bill>> GetByMotorcycle(int motoId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
                           SELECT b.* FROM Bill b
                           JOIN MotoInventory mi ON mi.bill_id = b.id
                           JOIN Motorcycles m ON m.id = mi.moto_id
                           WHERE m.id = @MotoId;
                           """;
        return await conn.QueryAsync<Bill>(sql, new { MotoId = motoId });
    }

    public async Task<User> GetUser(int billId)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT u.* FROM Bill b JOIN User u ON u.id = b.user_id WHERE b.id = @BillId";
        return await conn.QuerySingleAsync<User>(sql, new { BillId = billId });
    }

    public async Task<Bill?> GetById(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "SELECT * FROM Bill WHERE id = @Id";
        return await conn.QuerySingleOrDefaultAsync<Bill>(sql, new { Id = id });
    }

    public async Task<Bill> Create(CreateBillDto bill)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = """
            INSERT INTO Bill
                (user_id, amount, discount, payment_method)
            VALUES
                (@UserId,
                 CAST(@Amount::text AS MONEY),
                 CAST(@Discount::text AS MONEY),
                 @PaymentMethod)
            RETURNING *;
        """;
        return await conn.QuerySingleAsync<Bill>(sql, new
        {
            UserId        = bill.user_id,
            Amount        = bill.amount.ToString(),
            Discount      = bill.discount.ToString(),
            PaymentMethod = bill.payment_method
        });
    }

    public async Task<Bill> Update(UpdateBillDto bill, int id)
    {
        await using var conn = await GetConnectionAsync();
        var sb = new StringBuilder("UPDATE Bill SET ");
        var hasSet = false;

        if (bill.amount.HasValue)
        {
            sb.Append("amount = CAST(@Amount::text AS MONEY)");
            hasSet = true;
        }
        if (bill.discount.HasValue)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("discount = CAST(@Discount::text AS MONEY)");
            hasSet = true;
        }
        if (bill.payment_method != null)
        {
            if (hasSet) sb.Append(", ");
            sb.Append("payment_method = @PaymentMethod");
            hasSet = true;
        }
        
        if (!hasSet) throw new Exception("No fields to update");
        
        sb.Append(" WHERE id = @Id RETURNING *");
        
        return await conn.QuerySingleAsync<Bill>(sb.ToString(), new
        {
            Id            = id,
            Amount        = bill.amount.ToString(),
            Discount      = bill.discount.ToString(),
            PaymentMethod = bill.payment_method
        });
    }

    public async Task<bool> Delete(int id)
    {
        await using var conn = await GetConnectionAsync();
        const string sql = "DELETE FROM Bill WHERE id = @Id";
        var affected = await conn.ExecuteAsync(sql, new { Id = id });
        return affected > 0;
    }
}