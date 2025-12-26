
-- Checking stock for Product 1 before return
SELECT ProductID, ProductName, CurrentStock FROM dbo.Product WHERE ProductID = 1;

/* TEST CASE 8.9: Add Return Item */
USE FurnicolorDB;
GO

DECLARE @ActualReturnID INT;

-- 1. Create the Header first (sp_CreateSalesReturn)
EXEC dbo.sp_CreateSalesReturn 
    @SalesInvID = 97, 
    @CustomerID = 1, 
    @ReturnDate = '2025-12-26', 
    @Reason = N'Faulty Product', 
    @SalesReturnID = @ActualReturnID OUTPUT;

-- 2. Add the Item using the generated ID (sp_AddSalesReturnItem)
EXEC dbo.sp_AddSalesReturnItem 
    @SalesReturnID = @ActualReturnID, 
    @ProductID = 1, 
    @Quantity = 5.00;

-- 3. Verify the records
SELECT * FROM dbo.SalesReturnItem WHERE SalesReturnID = @ActualReturnID;

-- Stock should be updated via trigger after sp_AddSalesReturnItem
SELECT ProductID, ProductName, CurrentStock FROM dbo.Product WHERE ProductID = 1;