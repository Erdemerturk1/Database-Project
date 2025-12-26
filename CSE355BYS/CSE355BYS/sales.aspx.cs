using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
namespace CSE355BYS
{

    public partial class SalesPage : System.Web.UI.Page
    {
        private string ConStr => ConfigurationManager.ConnectionStrings["conStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
                LoadProducts();
                LoadCurrencies();
                txtInvDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }

        private void LoadCustomers()
        {
            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT CustomerID, CustomerName FROM dbo.Customer ORDER BY CustomerName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataValueField = "CustomerID";
                ddlCustomer.DataSource = dt;
                ddlCustomer.DataBind();
            }
        }

        private void LoadProducts()
        {
            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT ProductID, ProductName FROM dbo.Product ORDER BY ProductName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataSource = dt;
                ddlProduct.DataBind();
            }
        }

        private void LoadCurrencies()
        {
            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT CurrencyCode FROM dbo.Currency ORDER BY CurrencyCode", con))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlSalesCurrency.DataTextField = "CurrencyCode";
                ddlSalesCurrency.DataValueField = "CurrencyCode";
                ddlSalesCurrency.DataSource = dt;
                ddlSalesCurrency.DataBind();

                var tryItem = ddlSalesCurrency.Items.FindByValue("TRY");
                if (tryItem != null) ddlSalesCurrency.SelectedValue = "TRY";
            }
        }

        protected void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            lblCreateMsg.Text = "";

            int customerId = int.Parse(ddlCustomer.SelectedValue);
            string currency = ddlSalesCurrency.SelectedValue;
            DateTime invDate = ParseDateTime(txtInvDate.Text);
            bool isCredit = chkCredit.Checked;
            DateTime? dueDate = string.IsNullOrWhiteSpace(txtDueDate.Text) ? (DateTime?)null : ParseDateTime(txtDueDate.Text);

            int newId;

            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("dbo.sp_CreateSalesInvoice", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@InvoiceDate", invDate);
                cmd.Parameters.AddWithValue("@CurrencyCode", currency);
                cmd.Parameters.AddWithValue("@IsCredit", isCredit);
                cmd.Parameters.AddWithValue("@DueDate", (object)dueDate ?? DBNull.Value);

                var outId = new SqlParameter("@SalesInvID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);

                con.Open();
                cmd.ExecuteNonQuery();
                newId = (int)outId.Value;
            }

            lblCreateMsg.Text = "Created SalesInvID = " + newId;
            txtSalesInvID.Text = newId.ToString();
            txtQueryInvID.Text = newId.ToString();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            lblItemMsg.Text = "";

            int salesInvId = int.Parse(txtSalesInvID.Text.Trim());
            int productId = int.Parse(ddlProduct.SelectedValue);
            decimal qty = ParseDecimal(txtQty.Text);
            decimal unitPriceForeign = ParseDecimal(txtUnitPriceForeign.Text);

            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("dbo.sp_AddSalesInvoiceItem", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SalesInvID", salesInvId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                cmd.Parameters.AddWithValue("@UnitPriceForeign", unitPriceForeign);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblItemMsg.Text = "Item added. Click Query to view results.";
        }

        protected void btnQueryInv_Click(object sender, EventArgs e)
        {
            lblQueryMsg.Text = "";
            int salesInvId = int.Parse(txtQueryInvID.Text.Trim());

            using (var con = new SqlConnection(ConStr))
            {
                con.Open();

                using (var da = new SqlDataAdapter(
                    "SELECT SalesInvID, CustomerID, InvoiceDate, CurrencyCode, ExchangeRate, TotalAmountForeign, TotalAmountTRY, Status " +
                    "FROM dbo.SalesInvoice WHERE SalesInvID = @id", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesInvId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvHeader.DataSource = dt;
                    gvHeader.DataBind();
                }

                using (var da = new SqlDataAdapter(
                    "SELECT SalesInvItemID, ProductID, Quantity, UnitPriceForeign, UnitPriceTRY, TotalForeign, TotalTRY " +
                    "FROM dbo.SalesInvoiceItem WHERE SalesInvID = @id ORDER BY SalesInvItemID", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesInvId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvItems.DataSource = dt;
                    gvItems.DataBind();
                }

                using (var da = new SqlDataAdapter(
                    "SELECT TOP 200 TransactionID, ProductID, TransactionType, Quantity, TransactionDate, ReferenceType, ReferenceID, Notes " +
                    "FROM dbo.InventoryTransaction " +
                    "WHERE ReferenceType = 'SALE' AND ReferenceID = @id " +
                    "ORDER BY TransactionID DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesInvId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvInvTx.DataSource = dt;
                    gvInvTx.DataBind();
                }
            }

            lblQueryMsg.Text = "Queried SalesInvID = " + salesInvId;
        }

        private decimal ParseDecimal(string s)
        {
            s = (s ?? "").Trim().Replace(",", ".");
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }

        private DateTime ParseDateTime(string s)
        {
            s = (s ?? "").Trim();
            return DateTime.ParseExact(s, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
