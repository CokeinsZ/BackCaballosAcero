-- Infrastructure/Data/Queries/Statistics/SalesDistributionQuery.sql
-- Distribución de ventas por sede o por tipo de moto
SELECT
    b.name AS BranchName,
    COUNT(bm.id) AS SalesCount,
    COALESCE(SUM(bm.total), 0) AS TotalSales
FROM bill_motorcycle bm
         JOIN motoinventory mi ON bm.inventory_moto_id = mi.id
         JOIN branches b ON mi.branch_id = b.id
WHERE bm.created_at BETWEEN @startDate AND @endDate
  AND mi.branch_id = @branchId 
GROUP BY b.name
ORDER BY TotalSales DESC;
