using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IMotorcycleRepository
{
    Task<IEnumerable<Motorcycle>> GetByFilters(string brand, string model, int? branchId);
    public Task Add(Motorcycle entity);
}