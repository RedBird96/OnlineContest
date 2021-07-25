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

                //Getting Judgement data from yesterday
                DataTable judgementTable = SQL.GetDataTable("SELECT * FROM Judgement WHERE Date >= DATEADD(DAY, DATEDIFF(DAY, 1, GETDATE()), 0) AND Date < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)");

                //Evaluating for each user from yesterday
                foreach (DataRow row in judgementTable.Rows)
                {
                    int totalCorrect = 0;

                    //Assessing Question 1
                    P1 = SQL.ScalarQuery("SELECT P1 FROM CreateProblem WHERE QuestionNo = 1");
                    P2 = SQL.ScalarQuery("SELECT P2 FROM CreateProblem WHERE QuestionNo = 1");
                    P3 = SQL.ScalarQuery("SELECT P3 FROM CreateProblem WHERE QuestionNo = 1");

                    rate = Convert.ToDouble(P2);

                    if (P3 == "12:00 noon") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
                    else if (P3 == "12:30 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0);
                    else if (P3 == "01:00 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0);
                    else if (P3 == "01:30 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 30, 0);

                    if (assessment.AssessQ1(P1, rate, dateTime).Equals(row["Q1"].ToString())) totalCorrect++;

                    //Assessing Question 2
                    P1 = SQL.ScalarQuery("SELECT P1 FROM CreateProblem WHERE QuestionNo = 2");
                    P2 = SQL.ScalarQuery("SELECT P2 FROM CreateProblem WHERE QuestionNo = 2");
                    P3 = SQL.ScalarQuery("SELECT P3 FROM CreateProblem WHERE QuestionNo = 2");
                    P4 = SQL.ScalarQuery("SELECT P4 FROM CreateProblem WHERE QuestionNo = 2");

                    if (P2 == "above") isAbove = true;
                    else if (P2 == "below") isAbove = false;

                    rate = Convert.ToDouble(P3);

                    if (P4 == "today") days = 1;
                    else if (P4 == "this week") days = 7;
                    else if (P4 == "this month") days = 30;

                    if (assessment.AssessQ2(P1, isAbove, rate, days).Equals(row["Q2"].ToString())) totalCorrect++;

                    //Assessing Question 3
                    P1 = SQL.ScalarQuery("SELECT P1 FROM CreateProblem WHERE QuestionNo = 3");

                    if (assessment.AssessQ3(P1).Equals(row["Q3"].ToString())) totalCorrect++;

                    //Assessing Question 4
                    P1 = SQL.ScalarQuery("SELECT P1 FROM CreateProblem WHERE QuestionNo = 4");
                    P2 = SQL.ScalarQuery("SELECT P2 FROM CreateProblem WHERE QuestionNo = 4");

                    if (P2 == "12:00 noon") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
                    else if (P2 == "12:30 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0);
                    else if (P2 == "01:00 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 0, 0);
                    else if (P2 == "01:30 pm") dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 30, 0);

                    if (assessment.AssessQ4(P1, dateTime).Equals(row["Q4"].ToString())) totalCorrect++;

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
