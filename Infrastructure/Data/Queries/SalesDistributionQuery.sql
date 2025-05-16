-- Infrastructure/Data/Queries/Statistics/SalesDistributionQuery.sql
-- Distribución de ventas por sede o por tipo de moto

SELECT 
    b.name AS BranchName,
    COUNT(bm.id) AS SalesCount,
    COALESCE(SUM(bm.total), 0) AS TotalSales
FROM Bill_Motorcycle bm
JOIN Motoinventory mi ON bm.inventory_moto_id = mi.id
JOIN Branches b ON mi.branch_id = b.id
WHERE bm.created_at BETWEEN @startDate AND @endDate
GROUP BY b.name
ORDER BY TotalSales DESC
