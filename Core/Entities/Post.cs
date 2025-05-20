using System.ComponentModel;

namespace Core.Entities;

public class Post
{
    public int id { get; set; }
    public int branch_id { get; set; }
    public string? description { get; set; }
    public string? available_customizations { get; set; }
    public double price { get; set; }
    public string? status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

}

public enum PostStatus
{
    [Description("Available")]
    Available = 0,
    
    [Description("SoldOut")]
    SoldOut = 1
}