using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IMotoInventoryRepository
{
    Task<IEnumerable<MotoInventory>> GetByBranch(int branchId);
    Task<MotoInventory?> GetById(int id);
    Task<IEnumerable<MotoInventory>> GetByPostId(int postId);
    Task<MotoInventory> Create(CreateMotoInventoryDto dto);
    Task<MotoInventory?> Update(UpdateMotoInventoryDto dto, int id);
    Task<bool> ChangeStatus(int id, string status);
    Task<bool> Delete(int id);
}