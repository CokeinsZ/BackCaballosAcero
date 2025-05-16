using System.Text.Json;

namespace Core.DTOs;

public class CreateMotorcycleDto
{
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required string CC { get; set; }
    public string? Color { get; set; }
    public JsonElement? Details { get; set; }
}

public class UpdateMotorcycleDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? CC { get; set; }
    public string? Color { get; set; }
    public JsonElement? Details { get; set; }
}

public class FilterMotorcycleDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? CC { get; set; }
    public string? Color { get; set; }
}