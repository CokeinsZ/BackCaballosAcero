using Core.Entities;

namespace Core.Interfaces.RepositoriesInterfaces;

public interface ILogsRepository
{
    Task InsertOne(LogEntry entry);
    Task<IEnumerable<LogEntry>> GetLogs(string? entity, string? level, DateTime? fromUtc, DateTime? toUtc, int limit);
    Task<IEnumerable<LogEntry>> GetLogsByEntity(string entity);
    Task<IEnumerable<LogEntry>> GetLogsByLevel(string level);

}