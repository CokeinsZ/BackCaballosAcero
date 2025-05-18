using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IBranchRepository
{
    Task<IEnumerable<Branch>> GetAll();
    Task<Branch?> GetById(int id);
    Task<Branch> Create(CreateBranchDto dto);
    Task<Branch?> Update(UpdateBranchDto dto, int id);
    Task<bool> Delete(int id);
}