using System.Text;
using System.Text.Json;
using Core.Entities;
using Core.Interfaces.RepositoriesInterfaces;
using Dapper;

namespace Infrastructure.Data.Repositories;

public class LogsRepository : BaseConnection, ILogsRepository
    {
        public LogsRepository(string connectionString) : base(connectionString) { }

        public async Task InsertOne(LogEntry entry)
        {
            await using var conn = await GetConnectionAsync();
            const string sql = """
                INSERT INTO Logs
                    (timestamp, level, message, entity, exception, metadata)
                VALUES
                    (@Timestamp, @Level, @Message, @Entity, @Exception, CAST(@Metadata AS JSONB));
                """;

            var rawMetadata = entry.Metadata is null
                ? null
                : JsonSerializer.Serialize(entry.Metadata);

            await conn.ExecuteAsync(sql, new
            {
                entry.Timestamp,
                entry.Level,
                entry.Message,
                entry.Entity,
                entry.Exception,
                Metadata = rawMetadata
            });
        }

        public async Task<IEnumerable<LogEntry>> GetLogs(
            string? entity,
            string? level,
            DateTime? fromUtc,
            DateTime? toUtc,
            int limit)
        {
            var sb = new StringBuilder("""
                SELECT
                  timestamp AS Timestamp,
                  level     AS Level,
                  message   AS Message,
                  entity    AS Entity,
                  exception AS Exception,
                  metadata  AS Metadata
                FROM Logs
                """);

            var filters = new List<string>();
            if (entity   != null) filters.Add("entity    = @Entity");
            if (level    != null) filters.Add("level     = @Level");
            if (fromUtc  != null) filters.Add("timestamp >= @FromUtc");
            if (toUtc    != null) filters.Add("timestamp <= @ToUtc");

            if (filters.Count > 0)
                sb.Append(" WHERE ").Append(string.Join(" AND ", filters));

            sb.Append(" ORDER BY timestamp DESC LIMIT @Limit");

            await using var conn = await GetConnectionAsync();
            return await conn.QueryAsync<LogEntry>(sb.ToString(), new
            {
                Entity  = entity,
                Level   = level,
                FromUtc = fromUtc,
                ToUtc   = toUtc,
                Limit   = limit
            });
        }

        public Task<IEnumerable<LogEntry>> GetLogsByEntity(string entity)
            => GetLogs(entity, null, null, null, 100);

        public Task<IEnumerable<LogEntry>> GetLogsByLevel(string level)
            => GetLogs(null, level, null, null, 100);
    }