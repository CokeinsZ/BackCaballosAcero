using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;
    
    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet("branch/{branchId}")]
    public async Task<IActionResult> GetStatistics(int branchId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var statistics = await _statisticsService.GenerateStatistics(branchId, startDate, endDate);
        return Ok(statistics);
    }
    
    [HttpGet("history/{branchId}")]
    public async Task<IActionResult> GetHistoricalStatistics(int branchId)
    {
        var statistics = await _statisticsService.GetHistoricalStatistics(branchId);
        return Ok(statistics);
    }
}