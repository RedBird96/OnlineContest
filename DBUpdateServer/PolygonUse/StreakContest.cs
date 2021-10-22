using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonUse
{
    public static class StreakContest
    {
        static public void DoContest(Assessment assessment)
        {
            try
            {
                //Getting Judgement data from yesterday
                DataTable judgementTable = SQL.GetDataTable("SELECT * FROM StreakJudgement WHERE IsCalculated = 0");

                //Evaluating for each user from yesterday
                foreach (DataRow row in judgementTable.Rows)
                {
                    string submit_datetime = row["CreatedDate"].ToString();
                    string strStreak = row["Streak"].ToString();
                    string strSymbol = row["Stock"].ToString().Trim();
                    string strValue = row["Value"].ToString();
                    string strId = row["Id"].ToString();

                    string strAssess = "";
                    int nStreak = int.Parse(strStreak);

                    DateTime submit_DT = Convert.ToDateTime(submit_datetime);

                    if (submit_DT.Hour > 16)
                        submit_DT = submit_DT.AddDays(1);

                    strAssess = assessment.AssessStreak(strSymbol, submit_DT);

                    if (strAssess == strValue)
                        nStreak++;
                    else
                        nStreak = 0;

                    //Updating Score
                    SQL.NonScalarQuery("UPDATE StreakJudgement SET IsCalculated = 1, Streak = " + nStreak + " WHERE Id = " + strId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
