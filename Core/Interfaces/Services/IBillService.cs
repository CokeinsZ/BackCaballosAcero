using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IBillService
{
    Task<IEnumerable<Bill>> GetByBranch(int branchId);
    Task<IEnumerable<Bill>> GetByUser(int userId);
    Task<IEnumerable<Bill>> GetByMotorcycle(int motoId);
    Task<User> GetUser(int billId);
    Task<Bill?> GetById(int id);
    Task<Bill> Create(CreateBillDto dto);
    Task<Bill> Update(UpdateBillDto dto, int id);
    Task<bool> Cancel(int id);
    Task<bool> Delete(int id);
}