using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonUse
{
    public static class ExContest
    {
        static public void DoContest(Assessment assessment)
        {
            try
            {
                DateTime date = DateTime.Now;

                //Getting Judgement data from yesterday
                DataTable judgementTable = SQL.GetDataTable("SELECT * FROM PickEmJudgment WHERE IsCalculated = 0");
                Dictionary<int, Dictionary<string, double>> dicUserScore = new Dictionary<int, Dictionary<string, double>>();
//                Dictionary<string, double> dicUserScore = new Dictionary<string, double>();
                int nContestId = 0;

                //Evaluating for each user from yesterday
                foreach (DataRow row in judgementTable.Rows)
                {
                    string strId = row["Id"].ToString();
                    string strStocks = row["Stocks"].ToString();
                    string submit_datetime = row["CreatedDate"].ToString();
                    string strAmounts = row["Amount"].ToString();
                    string strUser = row["Username"].ToString();
                    nContestId = int.Parse(row["ContestId"].ToString());

                    DateTime submit_DT = Convert.ToDateTime(submit_datetime);
                    string[] strStocksArr = strStocks.Split(',');
                    if (strAmounts.Length == 0)
                    {
                        double score = assessment.AssessPickA(strStocksArr.ToList<string>(), submit_DT, date);
                        dicUserScore.Add(nContestId, new Dictionary<string, double>() { { strUser, score } });
                    }
                    else
                    {
                        double[] dAmountArr = Array.ConvertAll(strAmounts.Split(','), Double.Parse);
                        int score = (int)assessment.AssessPickB(strStocksArr.ToList<string>(), submit_DT, date, dAmountArr.ToList<double>());
                        dicUserScore.Add(nContestId, new Dictionary<string, double>() { { strUser, score } });
                    }
                    SQL.NonScalarQuery("UPDATE PickEmJudgment SET IsCalculated = 1 WHERE Id = " + strId);
                }
                if (nContestId != 0)
                {
                    foreach (var oneUserScoer in dicUserScore)
                    {
                        int ranking = oneUserScoer.Value.Count;
                        foreach (KeyValuePair<string, double> author in oneUserScoer.Value.OrderBy(key => key.Value))
                        {
                            SQL.NonScalarQuery("UPDATE PickEmResults SET Ranking = " + ranking + ", Score = " + author.Value + " WHERE Contestid = " + oneUserScoer.Key + " and Username = '" + author.Key + "'");
                            ranking = ranking - 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
