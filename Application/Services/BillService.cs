using System.Text.Json;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Email;
using Core.Interfaces.PopulatedEntities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class BillService: IBillService
{
    private readonly IBillRepository _repo;
    private readonly IMotoInventoryService _motoInventoryService;
    private readonly IPostService _postService;
    private readonly IEmailService _emailService;

    public BillService(IBillRepository repo, IMotoInventoryService motoInventoryService, IPostService postService, IEmailService emailService)
    {
        _repo = repo;
        _motoInventoryService = motoInventoryService;
        _postService = postService;
        _emailService = emailService;
    }

    private async Task<PopulatedBill> PopulateBill(Bill bill)
    {
        var user = await _repo.GetUser(bill.id);
        var motoInventories = await _repo.GetMotoInventory(bill.id);
        var populatedMotoInventories = new List<PopulatedMotoInventory>();
        foreach (var moto in motoInventories)
        {
            var populated = await _motoInventoryService.GetPopulatedById(moto.id);
            populatedMotoInventories.Add(populated!);
        }
        
        return new PopulatedBill(bill, user, populatedMotoInventories);
    }
    
    public async Task<IEnumerable<Bill>> GetByBranch(int branchId)
    {
        var bills = await _repo.GetByBranch(branchId);
        var populatedBills = new List<PopulatedBill>();
        foreach (var bill in bills)
        {
            var populated = await PopulateBill(bill);
            populatedBills.Add(populated);
        }
        
        return populatedBills;
    }

    public async Task<IEnumerable<Bill>> GetByUser(int userId)
    {
        var bills = await _repo.GetByUser(userId);
        var populatedBills = new List<PopulatedBill>();
        foreach (var bill in bills)
        {
            var populated = await PopulateBill(bill);
            populatedBills.Add(populated);
        }
        
        return populatedBills;
    }

    public async Task<IEnumerable<Bill>> GetByMotorcycle(int motoId)
    {
        var bills = await _repo.GetByMotorcycle(motoId);
        var populatedBills = new List<PopulatedBill>();
        foreach (var bill in bills)
        {
            var populated = await PopulateBill(bill);
            populatedBills.Add(populated);
        }
        
        return populatedBills;
    }

    public async Task<User> GetUser(int billId)
    {
        return await _repo.GetUser(billId);
    }

    public async Task<Bill?> GetById(int id)
    {
        var bill = await _repo.GetById(id);
        var populated = bill == null ? null : await PopulateBill(bill);
        return populated;
    }

    public async Task<Bill> Create(CreateBillDto dto)
    {
        var post = await _postService.GetById(dto.post_id);
        if (post == null)
            throw new Exception("Post not found");

        var postMotos = await _postService.GetMotoInventoriesByPostId(post.id);
        var available = postMotos
            .Where(m => m.status == "Available")
            .Take(dto.numberOfMotos)
            .ToList();

        if (available.Count < dto.numberOfMotos)
            throw new Exception("No hay suficientes motos disponibles");

        ValidateCustomizations(dto.customizations, post.available_customizations!);

        dto.amount = post.price * dto.numberOfMotos;

        var bill = await _repo.Create(dto);
        
        foreach (var moto in available)
        {
            await _motoInventoryService.Update(new UpdateMotoInventoryDto { customizations = dto.customizations, bill_id = bill.id}, moto.id);
            await _motoInventoryService.ChangeStatus(moto.id, "Sold");
        }
        
        return bill;
    }

    private void ValidateCustomizations(JsonElement? incomingJson, string allowedJson)
    {
        if (!incomingJson.HasValue)
            return;

        var incoming = incomingJson.Value;
        if (incoming.ValueKind != JsonValueKind.Object)
            throw new ArgumentException("Formato de customizaciones inválido");

        using var allowedDoc = JsonDocument.Parse(allowedJson);
        var allowed = allowedDoc.RootElement;
        if (allowed.ValueKind != JsonValueKind.Object)
            throw new Exception("El post no define customizaciones válidas");

        foreach (var prop in incoming.EnumerateObject())
        {
            if (!allowed.TryGetProperty(prop.Name, out var allowedValue)
                || allowedValue.GetString() != prop.Value.GetString())
            {
                throw new InvalidOperationException(
                    $"La customización «{prop.Name}: {prop.Value.GetString()}» no está permitida");
            }
        }
    }

    public async Task<Bill> Update(UpdateBillDto dto, int id)
    {
        var existing = await _repo.GetById(id);
        if (existing == null)
            throw new Exception("Bill not found");

        return await _repo.Update(dto, id);
    }

    public async Task<bool> Cancel(int id)
    {
        var user = await _repo.GetUser(id);
        
        //Free up related motorcycles in inventory
        var motoInventories = await _repo.GetMotoInventory(id);
        foreach (var moto in motoInventories)
        {
            await _motoInventoryService.Update(new UpdateMotoInventoryDto { customizations = null, bill_id = null}, moto.id);
            await _motoInventoryService.ChangeStatus(moto.id, "Available");
            await _emailService.SendCancellationNotification(user, moto);

        }

        return await _repo.Delete(id);
    }

    public async Task<bool> Delete(int id)
    {
        return await _repo.Delete(id);
    }
}