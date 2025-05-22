using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Application.Tools;

public static class MongoLogger
{
    private static IMongoCollection<LogEntry> _logsCollection;
    private static readonly object _lock = new object();

    // Configuración inicial
    public static void Initialize(IConfiguration config)
    {
        lock (_lock)
        {
            if (_logsCollection == null)
            {
                var connectionString = config["MongoDB:ConnectionString"];
                var databaseName = config["MongoDB:Database"];
                var collectionName = config["MongoDB:LogsCollection"];

                var client = new MongoClient(connectionString);
                var database = client.GetDatabase(databaseName);
                _logsCollection = database.GetCollection<LogEntry>(collectionName);
            }
        }
    }

    public static async Task LogInformation(string message, string entity, dynamic metadata = null)
    {
        await Log("INFO", message, entity, metadata);
    }

    public static async Task LogError(string message, string entity, Exception ex = null, dynamic metadata = null)
    {
        await Log("ERROR", message, entity, metadata, ex);
    }

    private static async void Log(string level, string message, string entity, dynamic metadata = null, Exception ex = null)
    {
        try
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                Entity = entity,
                Exception = ex?.ToString(),
                Metadata = metadata
            };

            await _logsCollection?.InsertOneAsync(logEntry);
        }
        catch (Exception loggerEx)
        {
            Console.WriteLine($"Fallo al guardar log: {loggerEx.Message}");
            Console.WriteLine($"Log original ({level}): {message}");
        }
    }
    
    public static async Task<List<LogEntry>> GetLogsAsync(
        string? entity      = null,
        string? level       = null,
        DateTime? fromUtc   = null,
        DateTime? toUtc     = null,
        int?    limit       = 100)
    {
        if (_logsCollection == null)
            throw new InvalidOperationException("MongoLogger no está inicializado.");

        var builder = Builders<LogEntry>.Filter;
        var filter  = builder.Empty;

        if (!string.IsNullOrWhiteSpace(entity))
            filter &= builder.Eq(le => le.Entity, entity);
        if (!string.IsNullOrWhiteSpace(level))
            filter &= builder.Eq(le => le.Level, level);
        if (fromUtc.HasValue)
            filter &= builder.Gte(le => le.Timestamp, fromUtc.Value);
        if (toUtc.HasValue)
            filter &= builder.Lte(le => le.Timestamp, toUtc.Value);

        return await _logsCollection
            .Find(filter)
            .SortByDescending(le => le.Timestamp)
            .Limit(limit.Value)
            .ToListAsync();
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Entity { get; set; }
        public string? Exception { get; set; }
        public dynamic? Metadata { get; set; }
    }
}