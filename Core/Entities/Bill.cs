namespace Core.Entities;

public class Bill
{
    public int id { get; set; }
    public int? user_id { get; set; }
    public double amount { get; set; }
    public double? discount { get; set; }
    public string? payment_method { get; set; } 
    public DateTime created_at { get; set; }
}