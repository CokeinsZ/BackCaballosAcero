using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services;

public interface IMotorcycleService
{
    Task<IEnumerable<Motorcycle>> GetAll();
    Task<Motorcycle?> GetById(int id);
    Task<IEnumerable<Motorcycle>> GetByFilters(FilterMotorcycleDto dto);
    Task<Motorcycle> Create(CreateMotorcycleDto dto);
    Task<Motorcycle> Update(UpdateMotorcycleDto dto, int id);
    Task Delete(int id);
}