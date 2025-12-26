/* TEST CASE: Add Item and Recalculate Totals */
USE FurnicolorDB;
GO

-- Adding 2 units of Product ID 1 at 500 USD each to the invoice created earlier
DECLARE @TargetInvID INT = 97; 

EXEC dbo.sp_AddSalesInvoiceItem 
    @SalesInvID = @TargetInvID, 
    @ProductID = 1, 
    @Quantity = 2, 
    @UnitPriceForeign = 500.00;

-- 1. Check the Invoice Items
SELECT * FROM dbo.SalesInvoiceItem WHERE SalesInvID = @TargetInvID;

-- 2. Check if the Header Totals were updated (Total will be 1000 USD / 30,000 TRY)
SELECT SalesInvID, TotalAmountForeign, TotalAmountTRY 
FROM dbo.SalesInvoice 
WHERE SalesInvID = @TargetInvID;