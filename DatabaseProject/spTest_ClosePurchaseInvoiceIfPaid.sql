/* TEST CASE : Closing a Purchase Invoice (Full Payment) */
USE FurnicolorDB;
GO

-- 1. ACTION: Make a full payment for Purchase Invoice 116 (30,000 TRY)
INSERT INTO dbo.SupplierPayment 
    (PurchaseInvID, SupplierID, PaymentDate, AmountTRY, AmountForeign, PaymentCurrency)
VALUES 
    (116, 1, GETDATE(), 30000, 1000, 'USD');

-- 2. EXECUTION: Run the procedure to update the status
EXEC dbo.sp_ClosePurchaseInvoiceIfPaid @PurchaseInvID = 116;

-- 3. RESULT: Check the final status (Should be 'PAID' now)
SELECT PurchaseInvID, TotalAmountTRY, Status 
FROM dbo.PurchaseInvoice 
WHERE PurchaseInvID = 116;