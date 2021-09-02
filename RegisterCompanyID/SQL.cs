using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterCompanyID
{
    public static class SQL
    {
        //private static SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=JudgementAppNew;Integrated Security=True;Pooling=False");// ReadCS().ToString()); 
        //private static SqlConnection con = new SqlConnection(@"workstation id=OnlineContest.mssql.somee.com;packet size=4096;user id=Bruce9623_SQLLogin_1;pwd=slwgidap1;data source=OnlineContest.mssql.somee.com;persist security info=False;initial catalog=OnlineContest");// ReadCS().ToString()); 
        private static SqlConnection con = new SqlConnection(@"workstation id=CompanyOnlineContest.mssql.somee.com;packet size=4096;user id=cjs9623_SQLLogin_1;pwd=9f8oably5f;data source=CompanyOnlineContest.mssql.somee.com;persist security info=False;initial catalog=CompanyOnlineContest");// ReadCS().ToString()); 

        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
        public static SqlConnection Con
        {
            get
            {
                return con;
            }
        }
        public static String ScalarQuery(String Query)
        {
            String Result = string.Empty;
            try
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                Con.Open();
                SqlCommand cmd = new SqlCommand(Query, Con);
                Result = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL " + ex.Message);
                // MessageBox.Show("SQL " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQL Scalar Query " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
            return Result;
        }
        public static bool NonScalarQuery(String Query)
        {
            bool queryStatus = false;
            try
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                Con.Open();
                SqlCommand cmd = new SqlCommand(Query, Con);
                cmd.ExecuteNonQuery();
                queryStatus = true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("SQL " + ex.Message);
                // MessageBox.Show("SQL " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SQL Non Scalar Query " + ex.Message);

            }
            finally
            {
                Con.Close();
            }
            return queryStatus;
        }
        public static DataTable GetDataTable(string Query)
        {
            if (SQL.Con.State == ConnectionState.Open)
            {
                SQL.Con.Close();
            }
            SQL.Con.Open();
            DataTable datasheets = new DataTable();
            SqlCommand command = new SqlCommand(Query, SQL.Con);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(datasheets);
            adapter.Dispose();
            return datasheets;
        }
    }
}
