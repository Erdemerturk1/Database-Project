using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CSE355BYS
{
    public partial class ProductPage : System.Web.UI.Page
    {
        private string ConStr => ConfigurationManager.ConnectionStrings["conStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadCurrencies();
            }
        }

        private void LoadCategories()
        {
            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT CategoryID, CategoryName FROM dbo.Category ORDER BY CategoryName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataSource = dt;
                ddlCategory.DataBind();
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

                ddlCurrency.DataTextField = "CurrencyCode";
                ddlCurrency.DataValueField = "CurrencyCode";
                ddlCurrency.DataSource = dt;
                ddlCurrency.DataBind();

                var tryItem = ddlCurrency.Items.FindByValue("TRY");
                if (tryItem != null) ddlCurrency.SelectedValue = "TRY";
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(ConStr))
            using (var da = new SqlDataAdapter(
                "SELECT TOP 200 ProductID, ProductName, BaseCurrency, BasePrice, PriceTRY, VATRate, CurrentStock " +
                "FROM dbo.Product ORDER BY ProductID DESC", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

    



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            int categoryId = int.Parse(ddlCategory.SelectedValue);
            string name = txtName.Text.Trim();
            string unit = ddlUnit.SelectedValue;
            string cur = ddlCurrency.SelectedValue;

            decimal basePrice = ParseDecimal(txtBasePrice.Text);
            int vat = int.Parse(ddlVat.SelectedValue);
            decimal stock = ParseDecimal(txtStock.Text);

            using (var con = new SqlConnection(ConStr))
            using (var cmd = new SqlCommand(
                "INSERT INTO dbo.Product(CategoryID, ProductName, Unit, BaseCurrency, BasePrice, VATRate, CurrentStock, IsActive) " +
                "VALUES(@cat, @name, @unit, @cur, @bp, @vat, @stk, 1)", con))
            {
                cmd.Parameters.AddWithValue("@cat", categoryId);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@cur", cur);
                cmd.Parameters.AddWithValue("@bp", basePrice);
                cmd.Parameters.AddWithValue("@vat", vat);
                cmd.Parameters.AddWithValue("@stk", stock);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Inserted. Click 'Query Products' to refresh.";
        }

        private decimal ParseDecimal(string s)
        {
            s = (s ?? "").Trim().Replace(",", ".");
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
