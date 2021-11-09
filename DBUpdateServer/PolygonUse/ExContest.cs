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
                Dictionary<int, List<Tuple<string, double>>> dicUserScore = new Dictionary<int, List<Tuple<string, double>>>();
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

                    DataTable questionTable = SQL.GetDataTable($"SELECT ContestExpiration, IsAllowNewContest FROM PickEmQuestions WHERE ContestId = {nContestId}");
                    bool bNewContest = (bool)questionTable.Rows[0]["IsAllowNewContest"];
                    string expiration_date = questionTable.Rows[0]["ContestExpiration"].ToString();

                    DataTable questionResult = SQL.GetDataTable($"SELECT Score FROM PickEmResults WHERE ContestId = {nContestId} and Username = '{strUser}'");
                    double score = 0;
                    if (questionResult.Rows[0]["Score"].ToString() != null)
                        score = double.Parse(questionResult.Rows[0]["Score"].ToString());

                    DateTime expire_DT = Convert.ToDateTime(expiration_date);

                    if (date <= expire_DT)
                    {
                        DateTime submit_DT = Convert.ToDateTime(submit_datetime);
                        if (bNewContest && submit_DT.Day != date.Day) // If user not submit today on new contest mode
                            continue;

                        if (!dicUserScore.ContainsKey(nContestId))
                        {
                            dicUserScore.Add(nContestId, new List<Tuple<string, double>>());
                        }

                        string[] strStocksArr = strStocks.Split(',');
                        if (strAmounts.Length == 0)
                        {
                            double score_today = assessment.AssessPickA(strStocksArr.ToList<string>(), submit_DT, date);
                            score += (score_today * 100);
                            dicUserScore[nContestId].Add(new Tuple<string, double>(strUser, Math.Round(score, 2)));
                        }
                        else
                        {
                            double[] dAmountArr = Array.ConvertAll(strAmounts.Split(','), Double.Parse);
                            int score_today = (int)assessment.AssessPickB(strStocksArr.ToList<string>(), submit_DT, date, dAmountArr.ToList<double>());
                            score += score_today;
                            dicUserScore[nContestId].Add(new Tuple<string, double>(strUser, score));
                        }
                    }
                    if (date >= expire_DT)
                        SQL.NonScalarQuery("UPDATE PickEmJudgment SET IsCalculated = 1 WHERE Id = " + strId);
                }
                if (dicUserScore.Count != 0)
                {
                    foreach (var oneUserScoer in dicUserScore)
                    {
                        int ranking = 1;
                        var lst = oneUserScoer.Value;
                        lst = lst.OrderByDescending(x => x.Item2).ToList();
                        foreach (var author in lst)
                        {
                            SQL.NonScalarQuery("UPDATE PickEmResults SET Ranking = " + ranking + ", Score = " + author.Item2 + " WHERE Contestid = " + oneUserScoer.Key + " and Username = '" + author.Item1 + "'");
                            ranking = ranking + 1;
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
