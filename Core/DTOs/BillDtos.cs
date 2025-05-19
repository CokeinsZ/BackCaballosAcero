using System.Text.Json;
using Core.Entities;

namespace Core.DTOs;

public class CreateBillDto
{
    public int numberOfMotos { get; set; }
    public int post_id { get; set; }
    public JsonElement? customizations { get; set; }
    public int user_id { get; set; }
    public double? amount { get; set; }
    public double? discount { get; set; }
    public string? payment_method { get; set; }
    public IEnumerable<int> moto_inventories_ids { get; set; }

}

public class UpdateBillDto
{
    public JsonElement? customizations { get; set; }
    public double? amount { get; set; }
    public double? discount { get; set; }
    public string? payment_method { get; set; }
    public IEnumerable<int>? moto_inventories_ids { get; set; }
}