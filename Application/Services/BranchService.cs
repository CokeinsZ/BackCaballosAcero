using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class BranchService: IBranchService
{
    private readonly IBranchRepository _repo;
    
    public BranchService(IBranchRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<IEnumerable<Branch>> GetAll()
    {
        return await _repo.GetAll();
    }

    public async Task<Branch?> GetById(int id)
    {
        return await _repo.GetById(id);
    }

    public async Task<Branch> Create(CreateBranchDto dto)
    {
        return await _repo.Create(dto);
    }

    public async Task<Branch?> Update(UpdateBranchDto dto, int id)
    {
        return await _repo.Update(dto, id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repo.Delete(id);
    }
}