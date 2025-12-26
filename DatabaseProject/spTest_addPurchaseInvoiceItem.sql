/* TEST CASE : Add Item and Recalculate Purchase Totals */

USE FurnicolorDB;
GO

-- Using the Purchase Invoice ID (116) generated in the previous step
DECLARE @TargetPurchID INT = 116; 

-- Adding 5 units of Product ID 1 at 200 USD each
EXEC dbo.sp_addPurchaseInvoiceItem 
    @PurchaseInvID = @TargetPurchID, 
    @ProductID = 1, 
    @Quantity = 5, 
    @UnitPriceForeign = 200.00;

-- 1. Verify the item was added
SELECT * FROM dbo.PurchaseInvoiceItem WHERE PurchaseInvID = @TargetPurchID;

-- 2. Verify the Header Update (Total will be 1000 USD / 30,000 TRY)
SELECT PurchaseInvID, TotalAmountForeign, TotalAmountTRY, Status 
FROM dbo.PurchaseInvoice 
WHERE PurchaseInvID = @TargetPurchID;