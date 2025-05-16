namespace Core.Interfaces.RepositoriesInterfaces;

public interface IStatisticsRepository
{
    Task<Statistics> GenerateStatisticsAsync(int branchId, DateTime startDate, DateTime endDate);
    Task<List<Statistics>> GetHistoricalStatisticsAsync(int branchId);
}