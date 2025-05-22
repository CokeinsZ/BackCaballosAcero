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

    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet]
    public async Task<IActionResult> GetLogs([FromQuery] LogsDto request)
    {
        var logs = await MongoLogger.GetLogsAsync(
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
        var logs = await MongoLogger.GetLogsAsync(entity);
        return Ok(logs);
    }
    
    [Authorize(Roles = "" + IUserRole.Admin + "," + IUserRole.Branch)]
    [HttpGet("level")]
    public async Task<IActionResult> GetByLevel([FromBody] string level)
    {
        var logs = await MongoLogger.GetLogsAsync(level);
        return Ok(logs);
    }

}