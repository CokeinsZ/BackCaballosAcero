-- Infrastructure/Data/Queries/Statistics/TotalIncomeQuery.sql
-- Obtiene los ingresos totales para un rango de fechas y sede específica

SELECT COALESCE(SUM(total), 0) AS total_income
FROM bill_motorcycle bm 
         JOIN motoinventory mi ON bm.inventory_moto_id = mi.id; -- ← minúscula