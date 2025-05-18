using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetByBranch(int branchId);
    Task<Post?> GetById(int id);
    Task<Post> Create(CreatePostDto dto);
    Task<Post?> Update(UpdatePostDto dto, int id);
    Task<bool> ChangeStatus(int id, string status);
    Task<bool> Delete(int id);
}