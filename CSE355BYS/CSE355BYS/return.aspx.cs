using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CSE355BYS
{
    public partial class ReturnPage : System.Web.UI.Page
    {
        private string ConStr => ConfigurationManager.ConnectionStrings["conStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
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
                ddlReturnProduct.DataTextField = "ProductName";
                ddlReturnProduct.DataValueField = "ProductID";
                ddlReturnProduct.DataSource = dt;
                ddlReturnProduct.DataBind();
            }
        }

        protected void btnCreateReturn_Click(object sender, EventArgs e)
        {
            lblReturnMsg.Text = "";

            int salesInvId = int.Parse(txtReturnSalesInvID.Text.Trim());
            int customerId = GetInvoiceCustomerId(salesInvId);
            string reason = string.IsNullOrWhiteSpace(txtReason.Text) ? null : txtReason.Text.Trim();

            int newReturnId;

            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("dbo.sp_CreateSalesReturn", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SalesInvID", salesInvId);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Reason", (object)reason ?? DBNull.Value);

                var outId = new SqlParameter("@SalesReturnID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);

                con.Open();
                cmd.ExecuteNonQuery();
                newReturnId = (int)outId.Value;
            }

            lblReturnMsg.Text = "Created SalesReturnID = " + newReturnId;
            txtSalesReturnID.Text = newReturnId.ToString();
            txtQueryReturnID.Text = newReturnId.ToString();
        }

        protected void btnAddReturnItem_Click(object sender, EventArgs e)
        {
            lblReturnItemMsg.Text = "";

            int salesReturnId = int.Parse(txtSalesReturnID.Text.Trim());
            int productId = int.Parse(ddlReturnProduct.SelectedValue);
            decimal qty = ParseDecimal(txtReturnQty.Text);

            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("dbo.sp_AddSalesReturnItem", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SalesReturnID", salesReturnId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", qty);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblReturnItemMsg.Text = "Return item added. Click Query to view.";
        }

        protected void btnQueryReturn_Click(object sender, EventArgs e)
        {
            int salesReturnId = int.Parse(txtQueryReturnID.Text.Trim());

            using (var con = new SqlConnection(ConStr))
            {
                con.Open();

                using (var da = new SqlDataAdapter(
                    "SELECT SalesReturnID, SalesInvID, CustomerID, ReturnDate, TotalAmountForeign, TotalAmountTRY, Reason " +
                    "FROM dbo.SalesReturn WHERE SalesReturnID = @id", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesReturnId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvReturnHeader.DataSource = dt;
                    gvReturnHeader.DataBind();
                }

                using (var da = new SqlDataAdapter(
                    "SELECT SalesReturnItemID, ProductID, Quantity " +  
                    "FROM dbo.SalesReturnItem WHERE SalesReturnID = @id ORDER BY SalesReturnItemID", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesReturnId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvReturnItems.DataSource = dt;
                    gvReturnItems.DataBind();
                }

                using (var da = new SqlDataAdapter(
                    "SELECT TOP 200 TransactionID, ProductID, TransactionType, Quantity, TransactionDate, ReferenceType, ReferenceID, Notes " +
                    "FROM dbo.InventoryTransaction " +
                    "WHERE ReferenceType = 'SALES_RETURN' AND ReferenceID = @id " +
                    "ORDER BY TransactionID DESC", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", salesReturnId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvReturnTx.DataSource = dt;
                    gvReturnTx.DataBind();
                }
            }
        }

        private int GetInvoiceCustomerId(int salesInvId)
        {
            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("SELECT CustomerID FROM dbo.SalesInvoice WHERE SalesInvID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", salesInvId);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private decimal ParseDecimal(string s)
        {
            s = (s ?? "").Trim().Replace(",", ".");
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
