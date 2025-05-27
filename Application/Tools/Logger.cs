
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;

namespace Application.Tools;

public class Logger
{
    private readonly ILogsRepository _logsRepository;

    public Logger(ILogsRepository logsRepository)
    {
        _logsRepository = logsRepository;
    }

    public async Task LogInformation(string message, string entity, dynamic metadata = null)
        => await Log("INFO", message, entity, metadata);

    public async Task LogError(string message, string entity, Exception ex = null, dynamic metadata = null)
        => await Log("ERROR", message, entity, metadata, ex);

    private async Task Log(string level, string message, string entity, dynamic metadata = null,
        Exception ex = null)
    {
        try
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level     = level,
                Message   = message,
                Entity    = entity,
                Exception = ex?.ToString(),
                Metadata  = metadata
            };

            await _logsRepository.InsertOne(logEntry);
        }
        catch (Exception loggerEx)
        {
            Console.WriteLine($"Fallo al guardar log: {loggerEx.Message}");
            Console.WriteLine($"Log original ({level}): {message}");
        }
    }

    public async Task<IEnumerable<LogEntry>> GetLogs(
        string? entity,
        string? level,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit)
        =>  await _logsRepository.GetLogs(entity, level, fromUtc, toUtc, limit);

    public async Task<IEnumerable<LogEntry>> GetLogsByEntity(string entity)
        => await _logsRepository.GetLogsByEntity(entity);

    public async Task<IEnumerable<LogEntry>> GetLogsByLevel(string level)
        => await _logsRepository.GetLogsByLevel(level);
}