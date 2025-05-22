namespace Core.DTOs;

public class LogsDto
{
    public string?   Entity  { get; set; }
    public string?   Level   { get; set; }
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc   { get; set; }
    public int       Limit   { get; set; } = 100;

}