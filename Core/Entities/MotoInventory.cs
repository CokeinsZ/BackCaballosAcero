using System.ComponentModel;
using System.Text.Json;

namespace Core.Entities;

public class MotoInventory
{
    public int id { get; set; }
    public int moto_id { get; set; }
    public int branch_id { get; set; }
    public int? post_id { get; set; }
    public int? bill_id { get; set; }
    public string? license_plate { get; set; }
    public string? km { get; set; }
    public string? customizations { get; set; }
    public string? status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

}

public enum MotoStatus 
{
    [Description("Available")] 
    Available = 0,
    
    [Description("Sold")]
    Sold = 1,
    
    [Description("Under Customization")]
    UnderCustomization = 2,
    
    [Description("Ready")]
    ReadyToDeliver = 3,
    
    [Description("Delivered")]
    Delivered = 4
}