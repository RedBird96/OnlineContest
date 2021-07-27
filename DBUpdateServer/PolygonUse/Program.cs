using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolygonUse
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Initializing variables
                Assessment assessment = new Assessment();
                string P1, P2, P3, P4;
                double rate;
                DateTime dateTime = new DateTime();
                bool isAbove = true;
                int days = 1;

                string txt_log = "";
                int txt_log_question_index = 0;
                //Getting Judgement data from yesterday
                DataTable judgementTable = SQL.GetDataTable("SELECT * FROM Judgement WHERE TotalCorrect IS NULL");

                txt_log = "[David 7.27 Test] ";
                //Evaluating for each user from yesterday
                foreach (DataRow row in judgementTable.Rows)
                {
                    int totalCorrect = 0;
                    string submit_datetime = row["Date"].ToString();
                    int question_cnt = int.Parse(row["ProblemNo"].ToString());
                    DateTime submit_DT = Convert.ToDateTime(submit_datetime);
                    int col_cnt = (row.Table.Columns.Count - 5) / 3;

                    for(int question_index = 1; question_index <= col_cnt; question_index ++)
                    {
                        string Qtag_Name = "Q" + question_index.ToString();
                        string Ttag_Name = "T" + question_index.ToString();
                        string Ptag_Name = "P" + question_index.ToString();

                        
                        if (row.IsNull(Qtag_Name) || row.IsNull(Ttag_Name) || row.IsNull(Ptag_Name))
                            continue;

                        string question_answer = row[Qtag_Name].ToString();
                        int question_type = int.Parse(row[Ttag_Name].ToString());
                        string[] parameter_arr = row[Ptag_Name].ToString().Split(',');

                        txt_log_question_index++;

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
                            else if (P3 == "03:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 30, 0);

                            if (assessment.AssessQ1(P1, rate, dateTime, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                txt_log = txt_log + $", Question{txt_log_question_index}:Right";
                            }
                            else
                            {
                                txt_log = txt_log + $", Question{txt_log_question_index}:Wrong";
                            }
                        }
                        
                        else if(question_type == 2)
                        {
                            //Assessing Question 2
                            P1 = parameter_arr[0];
                            P2 = parameter_arr[1];
                            P3 = parameter_arr[2];
                            P4 = parameter_arr[3];

                            if (P2 == "above") isAbove = true;
                            else if (P2 == "below") isAbove = false;

                            rate = Convert.ToDouble(P3);

                            if (P4 == "today") days = 1;
                            else if (P4 == "this week") days = 7;
                            else if (P4 == "this month") days = 30;

                            if (assessment.AssessQ2(P1, isAbove, days, rate, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                txt_log = txt_log + $",Question{txt_log_question_index}:Right";
                            }
                            else
                            {
                                txt_log = txt_log + $",Question{txt_log_question_index}:Wrong";
                            }
                        }
                        
                        else if(question_type == 3)
                        {
                            //Assessing Question 3
                            P1 = parameter_arr[0];

                            if (assessment.AssessQ3(P1, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                txt_log = txt_log + $",Question{txt_log_question_index}:Right";
                            }
                            else
                            {
                                txt_log = txt_log + $",Question{txt_log_question_index}:Wrong";
                            }
                        }
                        
                        else if(question_type == 4)
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
                            else if (P2 == "03:30 pm") dateTime = new DateTime(submit_DT.Year, submit_DT.Month, submit_DT.Day, 13, 30, 0);

                            if (assessment.AssessQ4(P1, dateTime, submit_DT).Equals(question_answer))
                            {
                                totalCorrect++;
                                txt_log = txt_log + $",Question{txt_log_question_index}:Right";
                            }
                            else
                            {
                                txt_log = txt_log + $",Question{txt_log_question_index}:Wrong";
                            }
                        }
                        
                    }
                    //Updating Score
                    SQL.NonScalarQuery("UPDATE Judgement SET TotalCorrect = " + totalCorrect + " WHERE Id = " + row["Id"]);
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
