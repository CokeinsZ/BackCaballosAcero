using System.Text.Json;

namespace Core.Entities;

public class Motorcycle
{
    public int id { get; set; }
    public required string brand { get; set; }
    public required string model { get; set; }
    public required string cc { get; set; }
    public string? color { get; set; }
    public JsonElement? details { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}