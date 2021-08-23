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
        public static string GetParameter(int questionNo, string ParameterNo,string ProblemName,long FKCompany)
        {
            return SQL.ScalarQuery("select " + ParameterNo + " from CreateProblem where questionNo = " + questionNo + " and ProblemName='" + ProblemName + "' and FKCompany=" + FKCompany + "");
        }
        public static bool CheckUser(string name,string ProblemName,long FKCompany)
        {
            string check = "";
            DateTime dateTime = DateTime.UtcNow.Date;
            check = SQL.ScalarQuery("SELECT CASE WHEN EXISTS (SELECT TOP 1 * FROM Judgement  WHERE Name = '" + name + "' and ProblemName='" + ProblemName + "' and FKCompany=" + FKCompany + "and TotalCorrect is NULL" /*" and date = (select CONVERT(datetime, '" + dateTime.ToString("yyyy/MM/dd") + "', 20))*/+") THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END");
            if (string.Equals("True", check))
            {
                return true;
            }
            else return false;
        }
        public static bool QuestionExists(string ProblemName,long QuestionNumber, string FKCompany)
        {
            string check = "";
            DateTime dateTime = DateTime.UtcNow.Date;
            check = SQL.ScalarQuery("SELECT CASE WHEN EXISTS (SELECT TOP 1 * FROM CreateProblem  WHERE ProblemName = '" + ProblemName + "' and FKCompany=" + FKCompany + "  and QuestionNo=" + QuestionNumber + ") THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END");
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