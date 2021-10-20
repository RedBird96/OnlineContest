using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonUse
{
    public static class CustomContest
    {
        public static void DoContest(Assessment assessment)
        {
            try
            {
                //Initializing variables
                string P1, P2, P3, P4;
                double rate;
                DateTime dateTime = new DateTime();
                bool isAbove = true;
                int days = 1;

                int txt_log_question_index = 0;

                //Getting Judgement data from yesterday
                DataTable judgementTable = SQL.GetDataTable("SELECT * FROM Judgement WHERE TotalCorrect IS NULL");
                //DataTable judgementTable = SQL.GetDataTable("SELECT * FROM Judgement WHERE Name='David'");

                //Evaluating for each user from yesterday
                foreach (DataRow row in judgementTable.Rows)
                {
                    bool skipFlag = false;
                    int totalCorrect = 0;
                    int totalquestions = 0;
                    string submit_datetime = row["Date"].ToString();
                    string fkCompany = row["FKCompany"].ToString();
                    string problemName = row["ProblemName"].ToString();
                    string userName = row["Name"].ToString();
                    string userEmailAdd = row["UserEmail"].ToString();
                    int question_cnt = int.Parse(row["ProblemNo"].ToString());
                    DateTime submit_DT = Convert.ToDateTime(submit_datetime);
                    DateTime weekend_DT = assessment.GetWeekendDate(submit_DT);
                    DateTime monthend_DT = assessment.GetMonthendDate(submit_DT);
                    int col_cnt = (row.Table.Columns.Count - 8) / 4;

                    for (int question_index = 0; question_index < col_cnt; question_index++)
                    {

                        int QIndex = (question_index * 4) + 8;
                        string Qtag_Name = "Q" + question_index.ToString();
                        string Ttag_Name = "T" + question_index.ToString();
                        string Ptag_Name = "P" + question_index.ToString();
                        string Rtag_Name = "R" + question_index.ToString();

                        if (row.ItemArray.ElementAt(QIndex).ToString().Length == 0)
                        {
                            continue;
                        }

                        string RcolumnHeaderName = row.Table.Columns[QIndex + 3].ColumnName;
                        string question_answer;//[Qtag_Name].ToString();
                        int question_type;// int.Parse(row[Ttag_Name].ToString());
                        string[] parameter_arr;// row[Ptag_Name].ToString().Split(',');
                        string strSQLFront = "update Judgement set " + RcolumnHeaderName + "=";
                        string strSQLBack = " where FKCompany = '" + fkCompany + "' and Name = '" + userName + "' and UserEmail='" + userEmailAdd + "' and ProblemName='" + problemName + "'";
                        string strSQL = "";
                        if (question_index > 4)
                        {
                            question_answer = row.ItemArray.ElementAt(QIndex).ToString();//[Qtag_Name].ToString();
                            parameter_arr = row.ItemArray.ElementAt(QIndex + 1).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                            question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 2).ToString());// int.Parse(row[Ttag_Name].ToString());

                        }
                        else
                        {
                            question_answer = row.ItemArray.ElementAt(QIndex).ToString();//[Qtag_Name].ToString();
                            question_type = int.Parse(row.ItemArray.ElementAt(QIndex + 1).ToString());// int.Parse(row[Ttag_Name].ToString());
                            parameter_arr = row.ItemArray.ElementAt(QIndex + 2).ToString().Split(',');// row[Ptag_Name].ToString().Split(',');
                        }

                        txt_log_question_index++;
                        totalquestions++;
                        if (question_type == 1)
                        {
                            //Assessing Question 1
                            P1 = parameter_arr[0];
                            P2 = parameter_arr[1];
                            P3 = parameter_arr[2];

                            rate = Convert.ToDouble(P2);

                            if (P3 == "10:00 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 10, 0, 0);
                            else if (P3 == "10:30 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 10, 30, 0);
                            else if (P3 == "11:00 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 11, 0, 0);
                            else if (P3 == "11:30 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 11, 30, 0);
                            else if (P3 == "12:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 12, 0, 0);
                            else if (P3 == "12:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 12, 30, 0);
                            else if (P3 == "01:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 0, 0);
                            else if (P3 == "01:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 30, 0);
                            else if (P3 == "02:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 14, 0, 0);
                            else if (P3 == "02:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 14, 30, 0);
                            else if (P3 == "03:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 15, 0, 0);
                            else if (P3 == "03:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 15, 30, 0);

                            if (assessment.AssessQ1(P1, rate, dateTime, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                strSQL = strSQLFront + "'True'" + strSQLBack;
                            }
                            else
                            {
                                strSQL = strSQLFront + "'False'" + strSQLBack;
                            }

                            SQL.NonScalarQuery(strSQL);
                        }

                        else if (question_type == 2)
                        {
                            //Assessing Question 2
                            P1 = parameter_arr[0];
                            P2 = parameter_arr[1];
                            P3 = parameter_arr[2];
                            P4 = parameter_arr[3];

                            if (P2 == "above") isAbove = true;
                            else if (P2 == "below") isAbove = false;

                            rate = Convert.ToDouble(P3);

                            if (P4 == "today")
                            {
                                days = 1;
                            }
                            else if (P4 == "this week")
                            {
                                days = 7;
                                if (DateTime.Now < weekend_DT)
                                {
                                    skipFlag = true;
                                    break;
                                }
                            }
                            else if (P4 == "this month")
                            {
                                days = 30;
                                if (DateTime.Now < monthend_DT)
                                {
                                    skipFlag = true;
                                    break;
                                }
                            }

                            if (assessment.AssessQ2(P1, isAbove, days, rate, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                strSQL = strSQLFront + "'True'" + strSQLBack;
                            }
                            else
                            {
                                strSQL = strSQLFront + "'False'" + strSQLBack;
                            }

                            SQL.NonScalarQuery(strSQL);
                        }

                        else if (question_type == 3)
                        {
                            //Assessing Question 3
                            P1 = parameter_arr[0];

                            if (assessment.AssessQ3(P1, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                strSQL = strSQLFront + "'True'" + strSQLBack;
                            }
                            else
                            {
                                strSQL = strSQLFront + "'False'" + strSQLBack;
                            }

                            SQL.NonScalarQuery(strSQL);
                        }

                        else if (question_type == 4)
                        {
                            //Assessing Question 4
                            P1 = parameter_arr[0];
                            P2 = parameter_arr[1];

                            if (P2 == "10:00 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 10, 0, 0);
                            else if (P2 == "10:30 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 10, 30, 0);
                            else if (P2 == "11:00 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 11, 0, 0);
                            else if (P2 == "11:30 am") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 11, 30, 0);
                            else if (P2 == "12:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 12, 0, 0);
                            else if (P2 == "12:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 12, 30, 0);
                            else if (P2 == "01:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 0, 0);
                            else if (P2 == "01:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 30, 0);
                            else if (P2 == "02:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 14, 0, 0);
                            else if (P2 == "02:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 14, 30, 0);
                            else if (P2 == "03:00 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 15, 0, 0);
                            else if (P2 == "03:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 15, 30, 0);

                            if (assessment.AssessQ4(P1, dateTime, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                strSQL = strSQLFront + "'True'" + strSQLBack;
                            }
                            else
                            {
                                strSQL = strSQLFront + "'False'" + strSQLBack;
                            }

                            SQL.NonScalarQuery(strSQL);
                        }
                        else if (question_type == 5)
                        {
                            //Assessing Question 5
                            P1 = parameter_arr[0];
                            P2 = parameter_arr[1];
                            P3 = parameter_arr[2];

                            if (P3 == "today")
                            {
                                days = 1;
                            }
                            else if (P3 == "this week")
                            {
                                days = 7;
                                if (DateTime.Now < weekend_DT)
                                {
                                    skipFlag = true;
                                    break;
                                }
                            }
                            else if (P3 == "this month")
                            {
                                days = 30;
                                if (DateTime.Now < monthend_DT)
                                {
                                    skipFlag = true;
                                    break;
                                }
                            }

                            if (assessment.AssessQ5(P1, P2, submit_DT, days).Equals(question_answer))
                            {
                                totalCorrect++;
                                strSQL = strSQLFront + "'True'" + strSQLBack;
                            }
                            else
                            {
                                strSQL = strSQLFront + "'False'" + strSQLBack;
                            }

                            SQL.NonScalarQuery(strSQL);

                        }

                    }
                    //Updating Score
                    if (!skipFlag)
                    {
                        SQL.NonScalarQuery("UPDATE Judgement SET TotalCorrect = " + totalCorrect + " , ProblemNo = " + totalquestions + " WHERE Id = " + row["Id"]);
                        SQL.NonScalarQuery("UPDATE CreateProblem SET IsExpired = 'True'" + " WHERE FKCompany = " + fkCompany + "and ProblemName = '" + problemName + "'");
                    }
                }
                Console.WriteLine("Score updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
