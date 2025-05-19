using Core.DTOs;
using Core.Entities;
using Core.Interfaces.PopulatedEntities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class PostService: IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMotoInventoryRepository _motoInventoryRepository;
    
    public PostService(IPostRepository postRepository, IBranchRepository branch, IMotoInventoryRepository motoInventoryRepository)
    {
        _postRepository = postRepository;
        _branchRepository = branch;
        _motoInventoryRepository = motoInventoryRepository;
    }
    
    public async Task<IEnumerable<PopulatedPost>> GetByBranch(int branchId)
    {
        var posts = await _postRepository.GetByBranch(branchId);
        var populatedPosts = new List<PopulatedPost>();
        foreach (var post in posts)
        {
            var motoInventories = await _motoInventoryRepository.GetByPostId(post.id);
            var branch = await _branchRepository.GetById(post.branch_id);
            populatedPosts.Add(new PopulatedPost(post, branch, motoInventories));
        }
        
        return populatedPosts;
    }

    public async Task<PopulatedPost?> GetById(int id)
    {
        var post = await _postRepository.GetById(id);
        
        if (post == null) return null;
        
        var motoInventories = await _motoInventoryRepository.GetByPostId(post.id);
        var branch = await _branchRepository.GetById(post.branch_id);
        return new PopulatedPost(post, branch, motoInventories);
    }

    public async Task<IEnumerable<MotoInventory>> GetMotoInventoriesByPostId(int postId)
    {
        var existing = await _postRepository.GetById(postId);
        if (existing == null) throw new Exception("Post not found");
        
        return await _motoInventoryRepository.GetByPostId(postId);
    }

    public async Task<Post> Create(CreatePostDto dto)
    {
        var post = await _postRepository.Create(dto);

        foreach (var motoInventoryId in dto.motoInventories)
        {
            var motoInventory = await _motoInventoryRepository.GetById(motoInventoryId);
            if (motoInventory == null)
            {
                await _postRepository.Delete(post.id);
                throw new Exception($"Moto in inventory with id {motoInventoryId} not found");

            }

            if (motoInventory.status != "Available")
            {
                await _postRepository.Delete(post.id);
                throw new Exception($"Moto in inventory with id {motoInventoryId} not available for sell");
            }
            
            await _motoInventoryRepository.Update(new UpdateMotoInventoryDto {post_id = post.id}, motoInventoryId);
            
        }
        
        return post;
    }

    public async Task<Post?> Update(UpdatePostDto dto, int id)
    {
        var existing = await _postRepository.GetById(id);
        if (existing == null)
            return null;

        if (dto.price == null
            && dto.description == null
            && !dto.availableCustomizations.HasValue
            && dto.motoInventories == null)
        {
            throw new Exception("No data to update");
        }

        // apply description, customizations or price update
        if (dto.price.HasValue
            || dto.description != null
            || dto.availableCustomizations.HasValue)
        {
            await _postRepository.Update(dto, id);
        }

        // re-assign motoInventories if provided
        if (dto.motoInventories != null)
        {
            // remove previous motoInventories from previous post
            var previousMotoInventories = await _motoInventoryRepository.GetByPostId(id);
            foreach (var previousMotoInventory in previousMotoInventories)
            {
                await _motoInventoryRepository.Update(new UpdateMotoInventoryDto {post_id = null}, previousMotoInventory.id);
            }
            
            // assign new motoInventories to new post
            foreach (var motoInventoryId in dto.motoInventories)
            {
                var motoInventory = await _motoInventoryRepository.GetById(motoInventoryId);
                if (motoInventory == null)
                {
                    throw new Exception($"Moto in inventory with id {motoInventoryId} not found");

                }

                if (motoInventory.status != "Available")
                {
                    throw new Exception($"Moto in inventory with id {motoInventoryId} not available for sell");
                }
            
                await _motoInventoryRepository.Update(new UpdateMotoInventoryDto {post_id = id}, motoInventoryId);
            
            }
        }
        
        return await GetById(id);
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