using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CSE355BYS
{
    public partial class PaymentPage : System.Web.UI.Page
    {
        private string ConStr => ConfigurationManager.ConnectionStrings["conStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
                LoadCurrencies();
                txtRateDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
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

                ddlCustomerPay.DataTextField = "CustomerName";
                ddlCustomerPay.DataValueField = "CustomerID";
                ddlCustomerPay.DataSource = dt;
                ddlCustomerPay.DataBind();
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

                ddlPayCurrency.DataTextField = "CurrencyCode";
                ddlPayCurrency.DataValueField = "CurrencyCode";
                ddlPayCurrency.DataSource = dt;
                ddlPayCurrency.DataBind();

                var tryItem = ddlPayCurrency.Items.FindByValue("TRY");
                if (tryItem != null) ddlPayCurrency.SelectedValue = "TRY";
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            lblPayMsg.Text = "";

            int? salesInvId = string.IsNullOrWhiteSpace(txtInvID.Text) ? (int?)null : int.Parse(txtInvID.Text.Trim());
            int customerId = int.Parse(ddlCustomerPay.SelectedValue);
            string payCur = ddlPayCurrency.SelectedValue;

            DateTime rateDate = DateTime.ParseExact(txtRateDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            decimal amountForeign = ParseDecimal(txtAmountForeign.Text);

            if (salesInvId.HasValue)
                customerId = GetInvoiceCustomerId(salesInvId.Value);

            decimal rate = GetTryRate(payCur, rateDate);

            try
            {
                using (var con = new SqlConnection(ConStr))
                using (var cmd = new SqlCommand("dbo.sp_RecordCustomerPayment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@SalesInvID", (object)salesInvId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PaymentCurrency", payCur);
                    cmd.Parameters.AddWithValue("@PaymentExchangeRate", rate);
                    cmd.Parameters.AddWithValue("@AmountForeign", amountForeign);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblPayMsg.Text = "Payment recorded.";
            }
            catch (SqlException ex)
            {
                lblPayMsg.Text = "ERROR: " + ex.Message;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            int salesInvId = int.Parse(txtQueryID.Text.Trim());

            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT SalesInvID, TotalAmountTRY, TotalAmountForeign, Status " +
                "FROM dbo.SalesInvoice WHERE SalesInvID = @id", con))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", salesInvId);
                var dt = new DataTable();
                da.Fill(dt);
                gvStatus.DataSource = dt;
                gvStatus.DataBind();
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

        private decimal GetTryRate(string currency, DateTime date)
        {
            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand("dbo.sp_GetTryRate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CurrencyCode", currency);
                cmd.Parameters.AddWithValue("@RateDate", date.Date);

                var outRate = new SqlParameter("@Rate", SqlDbType.Decimal)
                {
                    Precision = 18,
                    Scale = 6,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outRate);

                con.Open();
                cmd.ExecuteNonQuery();
                return (decimal)outRate.Value;
            }
        }

        private decimal ParseDecimal(string s)
        {
            s = (s ?? "").Trim().Replace(",", ".");
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
