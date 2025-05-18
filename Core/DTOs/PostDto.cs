using Core.Entities;

namespace Core.DTOs;

public class CreatePostDto
{
    
    public int branch_id { get; set; }
    public required double price { get; set; }
    public required IEnumerable<int> motoInventories { get; set; } 
}

public class UpdatePostDto
{

    public double? price { get; set; }
    public IEnumerable<int>? motoInventories { get; set; }
}

public class ChangePostStatusDto
{
    
    public required string status { get; set; }
}