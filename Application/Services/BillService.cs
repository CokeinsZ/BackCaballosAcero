using System.Text.Json;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.PopulatedEntities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class BillService: IBillService
{
    private readonly IBillRepository _repo;
    private readonly IMotoInventoryService _motoInventoryService;
    private readonly IPostService _postService;

    public BillService(IBillRepository repo, IMotoInventoryService motoInventoryService, IPostService postService)
    {
        _repo = repo;
        _motoInventoryService = motoInventoryService;
        _postService = postService;
    }
    
    public async Task<IEnumerable<Bill>> GetByBranch(int branchId)
    {
        return await _repo.GetByBranch(branchId);
    }

    public async Task<IEnumerable<Bill>> GetByUser(int userId)
    {
        return await _repo.GetByUser(userId);
    }

    public async Task<IEnumerable<Bill>> GetByMotorcycle(int motoId)
    {
        return await _repo.GetByMotorcycle(motoId);
    }

    public async Task<User> GetUser(int billId)
    {
        return await _repo.GetUser(billId);
    }

    public async Task<Bill?> GetById(int id)
    {
        return await _repo.GetById(id);
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

        ValidateCustomizations(dto.customizations, post.availableCustomizations!);

        foreach (var moto in available)
        {
            await _motoInventoryService.Update(new UpdateMotoInventoryDto { customizations = dto.customizations }, moto.id);
            await _motoInventoryService.ChangeStatus(moto.id, "Sold");
        }
        
        dto.amount = post.price * dto.numberOfMotos;

        return await _repo.Create(dto);
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

    public async Task<bool> Delete(int id)
    {
        return await _repo.Delete(id);
    }
}