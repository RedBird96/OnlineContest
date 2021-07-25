using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JudgementApp
{
    public class Main
    {
        public static string GetParameter(int questionNo, string ParameterNo)
        {
            return SQL.ScalarQuery("select " + ParameterNo + " from CreateProblem where questionNo = " + questionNo + "");
        }
        public static bool CheckUser(string name)
        {
            string check = "";
            DateTime dateTime = DateTime.UtcNow.Date;
            check = SQL.ScalarQuery("SELECT CASE WHEN EXISTS (SELECT TOP 1 * FROM Judgement  WHERE Name = '" + name + "' and date = (select CONVERT(datetime, '" + dateTime.ToString("yyyy/MM/dd") + "', 20))) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END");
            if (string.Equals("True", check))
            {
                return true;
            }
            else return false;
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