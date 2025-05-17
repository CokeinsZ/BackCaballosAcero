using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IMotorcycleRepository
{
    Task<IEnumerable<Motorcycle>> GetAll();
    Task<Motorcycle?> GetById(int id);
    Task<IEnumerable<Motorcycle>> GetByFilters(string? brand, string? model, string? cc, string? color);
    Task<Motorcycle> Create(CreateMotorcycleDto dto);
    Task Update(UpdateMotorcycleDto dto, int id);
    Task Delete(int id);

}