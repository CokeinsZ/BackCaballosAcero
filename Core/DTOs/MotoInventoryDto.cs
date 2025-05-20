using System.Text.Json;

namespace Core.DTOs;

public class CreateMotoInventoryDto
{
    public int moto_id { get; set; }
    public int branch_id { get; set; }
    public int? post_id { get; set; }
    public string? license_plate { get; set; }
    public string? km { get; set; }
    public JsonElement? customizations { get; set; }
}

public class UpdateMotoInventoryDto
{
    public int? post_id { get; set; }
    public int? bill_id { get; set; }
    public string? license_plate { get; set; }
    public string? km { get; set; }
    public JsonElement? customizations { get; set; }
}

public class ChangeMotoInventoryStatusDto
{
    public required string status { get; set; }
}