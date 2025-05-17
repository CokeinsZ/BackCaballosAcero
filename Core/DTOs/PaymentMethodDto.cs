namespace Core.DTOs;

public class CreateCardDto
{
    public int user_id { get; set; }
    public required string owner { get; set; }
    public required string pan { get; set; }
    public required string cvv { get; set; }
    public required string type { get; set; }
    public required string expiration_date { get; set; }
}

public class UpdateCardDto
{
    public string? owner { get; set; }
    public string? pan { get; set; }
    public string? cvv { get; set; }
    public string? type { get; set; }
    public string? expiration_date { get; set; }
}

public class ChangeCardStatusDto
{
    public required string status { get; set; }
}