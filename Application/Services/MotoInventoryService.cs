using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Email;
using Core.Interfaces.PopulatedEntities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class MotoInventoryService : IMotoInventoryService
{
    private readonly IMotoInventoryRepository _repo;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IBillRepository _billRepository;
    private readonly IEmailService _emailService;
    private readonly IBranchRepository _branchRepository;

    public MotoInventoryService(IMotoInventoryRepository repo, IBillRepository billRepository, IEmailService emailService, IBranchRepository branchRepository)
    {
        _repo = repo;
        _billRepository = billRepository;
        _emailService = emailService;
        _branchRepository = branchRepository;
    }

    public async Task<IEnumerable<MotoInventory>> GetByBranch(int branchId)
        => await _repo.GetByBranch(branchId);

    public async Task<MotoInventory?> GetById(int id)
        => await _repo.GetById(id);

    public async Task<PopulatedMotoInventory?> GetPopulatedById(int id)
    {
        var motoInventory = await GetById(id);
        if (motoInventory == null) return null;
        var motorcycle = await _motorcycleRepository.GetById(motoInventory.moto_id);
        var branch = await _branchRepository.GetById(motoInventory.branch_id);
        
        return new PopulatedMotoInventory(motoInventory, motorcycle, branch);
    }

    public async Task<IEnumerable<MotoInventory>> GetByPostId(int postId)
    {
        return await _repo.GetByPostId(postId);
    }

    public async Task<MotoInventory> Create(CreateMotoInventoryDto dto)
        => await _repo.Create(dto);

    public async Task<MotoInventory?> Update(UpdateMotoInventoryDto dto, int id)
        => await _repo.Update(dto, id);

    public async Task<bool> ChangeStatus(int id, string status)
    {
        var motoInventory = await GetById(id);
        if (motoInventory == null) throw new Exception("Moto not found");

        var ok = await _repo.ChangeStatus(id, status);
        
        if (ok && motoInventory.bill_id != null)
        {
            var user = await _billRepository.GetUser(motoInventory.bill_id.Value);
            var branch = await _branchRepository.GetById(motoInventory.branch_id);
            if (status.Equals("Sold")) await _emailService.SendPurchaseNotification(user, motoInventory, branch);
            else if (status.Equals("Ready")) await _emailService.SendReadyToPickupEmail(user, motoInventory, branch);
            else await _emailService.SendStatusUpdateEmail(user, motoInventory, branch);
        }

        return ok;
    }

    public async Task<bool> AsignBill(int id, int billId)
    {
        var motoInventory = await GetById(id);
        if (motoInventory.bill_id != null) throw new Exception("Moto already has a bill");
        if (motoInventory.status != "Available") throw new Exception("Moto not available");
        
        return await _repo.AsignBill(id, billId);
    }

    public async Task<bool> Delete(int id)
        => await _repo.Delete(id);
}