public async Task<Statistics> GenerateStatisticsAsync(int branchId, DateTime startDate, DateTime endDate)
{
    var parameters = new { branchId, startDate, endDate };
    
    var totalIncome = await QueryFirstOrDefaultAsync<decimal>(
        await LoadQueryAsync("TotalIncomeQuery"), parameters);
        
    var bestSeller = await QueryFirstOrDefaultAsync<string>(
        await LoadQueryAsync("BestSellerQuery"), parameters);
        
    var distribution = await QueryAsync<(string BranchName, int SalesCount, decimal TotalSales)>(
        await LoadQueryAsync("SalesDistributionQuery"), parameters);
    
    // Formatear distribución para almacenamiento
    var distributionText = string.Join(", ", 
        distribution.Select(d => $"{d.BranchName}: {d.SalesCount} ventas (${d.TotalSales})"));
    
    return new Statistics
    {
        TotalIncome = totalIncome,
        BestSeller = bestSeller ?? "No hay ventas registradas",
        SellingDistribution = distributionText,
        CreatedAt = DateTime.Now,
        BranchId = branchId
    };
}