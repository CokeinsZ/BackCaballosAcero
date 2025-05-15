using System.ComponentModel;

namespace Core.Entities;

public class User
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string last_name { get; set; }
    public required string identification_document { get; set; }
    public required string country { get; set; }
    public required string city { get; set; }
    public string? address { get; set; }
    public required string password { get; set; }
    public required string email { get; set; }
    public string? phone_number { get; set; }
    public string? status { get; set; }
    public int? role_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

}

public enum UserStatus
{
    [Description("Active")]
    Active = 0,
    
    [Description("Not Verified")]
    NotVerified = 1,
    
    [Description("Inactive")]
    Inactive = 2,
    
    [Description("Banned")]
    Banned = 3
}