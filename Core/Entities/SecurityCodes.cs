namespace Core.Entities;

public class SecurityCodes
{
    public int id { get; set; }
    public int user_id { get; set; }
    public required string code { get; set; }
    public DateTime created_at { get; set; }
    public DateTime expires_at { get; set; }
}