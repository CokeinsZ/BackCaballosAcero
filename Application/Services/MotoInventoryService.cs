using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class MotoInventoryService : IMotoInventoryService
{
    private readonly IMotoInventoryRepository _repo;

    public MotoInventoryService(IMotoInventoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<MotoInventory>> GetByBranch(int branchId)
        => await _repo.GetByBranch(branchId);

    public async Task<MotoInventory?> GetById(int id)
        => await _repo.GetById(id);

    public async Task<MotoInventory> Create(CreateMotoInventoryDto dto)
        => await _repo.Create(dto);

    public async Task<MotoInventory?> Update(UpdateMotoInventoryDto dto, int id)
        => await _repo.Update(dto, id);

    public async Task<bool> ChangeStatus(int id, string status)
        => await _repo.ChangeStatus(id, status);

    public async Task<bool> Delete(int id)
        => await _repo.Delete(id);
}