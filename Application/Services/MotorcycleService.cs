using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Services;

namespace Application.Services;

public class MotorcycleService: IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<IEnumerable<Motorcycle>> GetAll()
    {
        return await _motorcycleRepository.GetAll();
    }

    public async Task<Motorcycle?> GetById(int id)
    {
        return await _motorcycleRepository.GetById(id);
    }

    public async Task<IEnumerable<Motorcycle>> GetByFilters(FilterMotorcycleDto dto)
    {
        return await _motorcycleRepository.GetByFilters(dto.Brand, dto.Model, dto.CC, dto.Color);
    }

    public async Task<Motorcycle> Create(CreateMotorcycleDto dto)
    {
        return await _motorcycleRepository.Create(dto);
    }

    public async Task<Motorcycle> Update(UpdateMotorcycleDto dto, int id)
    {
        await _motorcycleRepository.Update(dto, id);
        var updated = await _motorcycleRepository.GetById(id);
        if (updated is null)
            throw new KeyNotFoundException($"Motorcycle with id {id} not found.");
        return updated;
    }

    public async Task Delete(int id)
    {
        await _motorcycleRepository.Delete(id);
    }
        
}