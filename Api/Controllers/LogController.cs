using Application.Tools;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/logs")]
public class LogController : ControllerBase
{
    private readonly Logger _logger;

    public LogController(Logger logger)
    {
        _logger = logger;
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet]
    public async Task<IActionResult> GetLogs([FromQuery] LogsDto request)
    {
        var logs = await _logger.GetLogs(
            request.Entity,
            request.Level,
            request.FromUtc,
            request.ToUtc,
            request.Limit
        );
        return Ok(logs);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("entity")]
    public async Task<IActionResult> GetByEntity([FromBody] string entity)
    {
        var logs = await _logger.GetLogsByEntity(entity);
        return Ok(logs);
    }

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("level")]
    public async Task<IActionResult> GetByLevel([FromBody] string level)
    {
        var logs = await _logger.GetLogsByLevel(level);
        return Ok(logs);
    }
}