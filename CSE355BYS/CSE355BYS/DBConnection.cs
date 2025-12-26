using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CSE355BYS
{
    public class DBConnection
    {
        private readonly string connectionString;

        public DBConnection()
        {
            connectionString = ConfigurationManager.ConnectionStrings["conStr"].ToString();
        }

        public DataSet getSelect(string sqlstr)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(sqlstr, con))
            {
                da.Fill(ds);
            }

            return ds;
        }

        public bool execute(string sqlstr)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sqlstr, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
