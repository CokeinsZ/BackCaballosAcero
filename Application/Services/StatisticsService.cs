namespace Application.Services;

public class StatisticsService
{
    private readonly IStatisticsRepository _statisticsRepository;
    
    public StatisticsService(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }
    
    public async Task<StatisticsDto> GenerateStatistics(int branchId, DateTime startDate, DateTime endDate)
    {
        var statistics = await _statisticsRepository.GenerateStatisticsAsync(branchId, startDate, endDate);
        
        return new StatisticsDto
        {
            TotalIncome = statistics.TotalIncome,
            BestSellingProduct = statistics.BestSellingProduct,
            SalesDistribution = statistics.SalesDistribution,
            GenerationDate = DateTime.Now,
            BranchName = statistics.Branch?.Direccion ?? "Todas las sedes"
        };
    }
    
    public async Task<List<StatisticsDto>> GetHistoricalStatistics(int branchId)
    {
        // Similar al anterior pero para múltiples registros
    }
}