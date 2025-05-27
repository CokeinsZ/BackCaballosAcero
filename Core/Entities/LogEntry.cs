namespace Core.Entities;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string Entity { get; set; }
    public string? Exception { get; set; }
    public dynamic? Metadata { get; set; }
}