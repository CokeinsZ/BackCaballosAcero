using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Statistics
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("total_income")]
    public decimal TotalIncome { get; set; }
    
    [Column("best_seller")]
    public string BestSeller { get; set; }
    
    [Column("selling_distribution")]
    public string SellingDistribution { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("branch_id")]
    public int BranchId { get; set; }
}