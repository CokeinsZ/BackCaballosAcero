using System.Text.Json;
using Core.Entities;

namespace Core.DTOs;

public class CreatePostDto
{
    public int branch_id { get; set; }
    public string? description { get; set; }
    public JsonElement? availableCustomizations { get; set; }
    public required double price { get; set; }
    public required IEnumerable<int> motoInventories { get; set; } 
}

public class UpdatePostDto
{
    public string? description { get; set; }
    public JsonElement? availableCustomizations { get; set; }
    public double? price { get; set; }
    public IEnumerable<int>? motoInventories { get; set; }
}

public class ChangePostStatusDto
{
    public required string status { get; set; }
}