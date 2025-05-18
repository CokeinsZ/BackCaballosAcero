using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class PostService: IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMotoInventoryRepository _motoInventoryRepository;
    
    public PostService(IPostRepository postRepository, IMotoInventoryRepository motoInventoryRepository)
    {
        _postRepository = postRepository;
        _motoInventoryRepository = motoInventoryRepository;
    }
    
    public async Task<IEnumerable<Post>> GetByBranch(int branchId)
    {
        return await _postRepository.GetByBranch(branchId);
    }

    public async Task<Post?> GetById(int id)
    {
        return await _postRepository.GetById(id);
    }

    public async Task<Post> Create(CreatePostDto dto)
    {
        var post = await _postRepository.Create(dto);

        foreach (var motoInventoryId in dto.motoInventories)
        {
            var motoInventory = await _motoInventoryRepository.GetById(motoInventoryId);
            if (motoInventory == null)
                throw new Exception($"Moto in inventory with id {motoInventoryId} not found");

            if (motoInventory.status != "Available")
                throw new Exception($"Moto in inventory with id {motoInventoryId} not available for sell");
            
            await _motoInventoryRepository.Update(new UpdateMotoInventoryDto {post_id = post.id}, motoInventoryId);
            
        }
        
        return post;
    }

    public async Task<Post?> Update(UpdatePostDto dto, int id)
    {
        if (dto.price == null && dto.motoInventories == null) throw new Exception("No data to update");
        
        if (dto.price != null)
            await _postRepository.Update(new UpdatePostDto {price = dto.price}, id);

        if (dto.motoInventories != null)
        {
            foreach (var motoInventoryId in dto.motoInventories)
            {
                var motoInventory = await _motoInventoryRepository.GetById(motoInventoryId);
                if (motoInventory == null)
                    throw new Exception($"Moto in inventory with id {motoInventoryId} not found");

                if (motoInventory.status != "Available")
                    throw new Exception($"Moto in inventory with id {motoInventoryId} not available for sell");
            
                await _motoInventoryRepository.Update(new UpdateMotoInventoryDto {post_id = id}, motoInventoryId);
            
            }
        }
        
        return await _postRepository.GetById(id);
    }

    public async Task<bool> ChangeStatus(int id, string status)
    {
        return await _postRepository.ChangeStatus(id, status);
    }

    public async Task<bool> Delete(int id)
    {
        return await _postRepository.Delete(id);
    }
}