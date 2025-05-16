-- Infrastructure/Data/Queries/Statistics/TotalIncomeQuery.sql
-- Obtiene los ingresos totales para un rango de fechas y sede específica

SELECT COALESCE(SUM(bm.total), 0) AS TotalIncome
FROM Bill_Motorcycle bm
JOIN Motoinventory mi ON bm.inventory_moto_id = mi.id
WHERE bm.created_at BETWEEN @startDate AND @endDate
AND mi.branch_id = @branchId