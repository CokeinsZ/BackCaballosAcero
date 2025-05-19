using Core.DTOs;
using Core.Entities;
using Core.Interfaces.PopulatedEntities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IBillRepository
{
    Task<IEnumerable<Bill>> GetByBranch(int branchId);
    Task<IEnumerable<Bill>> GetByUser(int userId);
    Task<IEnumerable<Bill>> GetByMotorcycle(int motoId);
    Task<User> GetUser(int billId);
    Task<Bill?> GetById(int id);
    Task<Bill> Create(CreateBillDto bill);
    Task<Bill> Update(UpdateBillDto bill, int id);
    Task<bool> Delete(int id);
}