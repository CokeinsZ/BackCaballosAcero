namespace Core.DTOs;

public class StatisticsDto
{
    public double TotalIncome { get; set; }
    public string BestSellingProduct { get; set; }
    public List<string> SalesDistribution { get; set; }
    public DateTime GenerationDate { get; set; }
    public string BranchName { get; set; }
}