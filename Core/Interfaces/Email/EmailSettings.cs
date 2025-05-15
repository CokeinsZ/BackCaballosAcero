namespace Core.Interfaces.Email;

public class EmailSettings
{
    public required string SmtpServer { get; set; }
    public int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string FromName { get; set; }
}