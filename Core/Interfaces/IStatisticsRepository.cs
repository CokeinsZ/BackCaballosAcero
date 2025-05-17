// Core/Interfaces/RepositoriesInterfaces/IStatisticsRepository.cs
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface IStatisticsRepository
{
    Task<Statistics?> GenerateStatisticsAsync(int branchId, DateTime startDate, DateTime endDate);
}