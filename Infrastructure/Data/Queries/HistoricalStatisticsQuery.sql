-- Infrastructure/Data/Queries/Statistics/HistoricalStatisticsQuery.sql
-- Estadísticas históricas agrupadas por mes

SELECT 
    DATE_TRUNC('month', bm.created_at) AS Month,
    COUNT(bm.id) AS TotalSales,
    COALESCE(SUM(bm.total), 0) AS TotalIncome,
    (
        SELECT m.model 
        FROM Motorcycles m
        JOIN Motoinventory mi ON m.id = mi.moto_id
        JOIN Bill_Motorcycle b ON mi.id = b.inventory_moto_id
        WHERE DATE_TRUNC('month', b.created_at) = DATE_TRUNC('month', bm.created_at)
        AND mi.branch_id = @branchId
        GROUP BY m.model
        ORDER BY COUNT(b.id) DESC
        LIMIT 1
    ) AS BestSellerModel
FROM Bill_Motorcycle bm
JOIN Motoinventory mi ON bm.inventory_moto_id = mi.id
WHERE mi.branch_id = @branchId
GROUP BY Month
ORDER BY Month DESC
LIMIT 12