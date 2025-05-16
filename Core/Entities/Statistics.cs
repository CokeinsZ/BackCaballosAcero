namespace Core.Entities;

public class Statistics
{
    public int Id { get; set; }
    public decimal TotalIncome { get; set; }
    public string BestSeller { get; set; } 
    public string SellingDistribution { get; set; } 
    public DateTime CreatedAt { get; set; }
    
    public int BranchId { get; set; }
    public Branch Branch { get; set; }
}