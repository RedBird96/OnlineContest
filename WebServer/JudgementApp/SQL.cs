﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace JudgementApp
{
    public static class SQL
    {
        //private static SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=JudgementAppNew;Integrated Security=True;Pooling=False");// ReadCS().ToString()); 
        private static SqlConnection con = new SqlConnection(@"workstation id=OnlineContest.mssql.somee.com;packet size=4096;user id=Bruce9623_SQLLogin_1;pwd=slwgidap1;data source=OnlineContest.mssql.somee.com;persist security info=False;initial catalog=OnlineContest");// ReadCS().ToString()); 
        //private static SqlConnection con = new SqlConnection(@"workstation id=CompanyOnlineContest.mssql.somee.com;packet size=4096;user id=cjs9623_SQLLogin_1;pwd=9f8oably5f;data source=CompanyOnlineContest.mssql.somee.com;persist security info=False;initial catalog=CompanyOnlineContest");// ReadCS().ToString()); 

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
                using (SqlConnection con = new SqlConnection(SQL.Con.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        Result = cmd.ExecuteScalar().ToString();
                    }
                }
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

                using (SqlConnection con = new SqlConnection(SQL.Con.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(Query))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        queryStatus = true;
                    }
                }
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
    }
}
