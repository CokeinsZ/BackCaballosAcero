using Core.DTOs;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;

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
            // Conversión explícita de decimal a double
            TotalIncome = Convert.ToDouble(statistics.TotalIncome),
            
            // Mapeo de nombres diferentes (BestSeller -> BestSellingProduct)
            BestSellingProduct = statistics.BestSeller,
            
            // Convertir string a List<string> (si viene separado por comas)
            SalesDistribution = statistics.SellingDistribution?.Split(',').ToList() ?? new List<string>(),
            
            GenerationDate = DateTime.Now,
            
            // Obtener nombre de la sede usando el ID (requeriría una nueva consulta)
            BranchName = $"Sede {statistics.BranchId}" // Temporal
        };
    }
    
    public async Task<List<StatisticsDto>> GetHistoricalStatistics(int branchId)
    {
        // Implementación similar
        throw new NotImplementedException();
    }
}