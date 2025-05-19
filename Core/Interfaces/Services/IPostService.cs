using Core.DTOs;
using Core.Entities;
using Core.Interfaces.PopulatedEntities;

namespace Core.Interfaces.Services;

public interface IPostService
{
    Task<IEnumerable<PopulatedPost>> GetByBranch(int branchId);
    Task<PopulatedPost?> GetById(int id);
    Task<Post> Create(CreatePostDto dto);
    Task<Post?> Update(UpdatePostDto dto, int id);
    Task<bool> ChangeStatus(int id, string status);
    Task<bool> Delete(int id);
}