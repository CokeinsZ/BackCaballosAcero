namespace Core.DTOs;

public class CreateBranchDto
{
    public required string name { get; set; }
    public required string country { get; set; }
    public required string city { get; set; }
    public string? address { get; set; }
}

public class UpdateBranchDto
{
    public string? name { get; set; }
    public string? country { get; set; }
    public string? city { get; set; }
    public string? address { get; set; }
}