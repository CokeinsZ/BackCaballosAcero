-- Infrastructure/Data/Queries/Statistics/BestSellerQuery.sql
-- Identifica el producto más vendido (moto) por cantidad de ventas

SELECT 
    m.brand AS Brand,
    m.model AS Model,
    COUNT(bm.id) AS SalesCount
FROM Bill_Motorcycle bm
JOIN Motoinventory mi ON bm.inventory_moto_id = mi.id
JOIN Motorcycles m ON mi.moto_id = m.id
WHERE bm.created_at BETWEEN @startDate AND @endDate
AND mi.branch_id = @branchId
GROUP BY m.brand, m.model
ORDER BY SalesCount DESC
LIMIT 1