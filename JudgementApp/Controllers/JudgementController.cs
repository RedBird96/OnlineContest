using JudgementApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JudgementApp.Controllers
{
    public class JudgementController : Controller
    {
        // GET: Judgement
        public ActionResult Index()
        {
            //var model = new Data();
            //model.Q1_P1 = Main.GetParameter(1, "p1");
            return View();
        }
        public ActionResult CreateProblem(Data data)
        {
            //To take the symbol from the website comment the below code 
            var model = new List<Data>();
            string[] symbolNames = { "SPY", "QQQ", "IWM", "TLT", "TSLA", "NFLX", "AAPL", "AMZN", "FB", "GOOGL", "NVDA" };
            int id = 0;
            foreach (var line in symbolNames)
            {
                var data1 = new Data();
                data1.SymbolName = line;
                data1.Id = id++;
                model.Add(data1);
            }
            return View(model);

            //uncomment this code to get the symbol from the site
            //var model = new List<Data>();
            //var url = "http://oatsreportable.finra.org/OATSReportableSecurities-SOD.txt";
            //var results = (new WebClient()).DownloadString(url);

            //var Lines = results.Split('\n');
            //int id = 0;
            //foreach (var line in Lines)
            //{
            //    var data1 = new Data();
            //    data1.SymbolName = line.Split('|')[0];
            //    data1.Id = id++;
            //    model.Add(data1);
            //}
            //return View(model);

        }
        public ActionResult Leaderboard()
        {
            var leaderboard = new List<Leaderboard>();

            var results = Main.GetDataTable("select Name  from Judgement group by Name");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                int totalCorrect = 0;
                int.TryParse(SQL.ScalarQuery("select SUM(TotalCorrect) from Judgement where name = '" + row.Username + "'"), out totalCorrect);
                row.TotalCorrect = totalCorrect;
                row.ContestAttempted = Convert.ToInt32(SQL.ScalarQuery("select Count(*) from Judgement where Name = '" + row.Username + "'"));
                double winPer = 0;
                winPer = (row.TotalCorrect *100) / (row.ContestAttempted *4);
                row.WinPercentage = winPer.ToString();

                leaderboard.Add(row);
            }

            return View(leaderboard);
        }
        public ActionResult saveJudgement(JudgementParameter result)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            if (Main.CheckUser(result.UserName))
            {
                SQL.NonScalarQuery("update Judgement set Q1 ='" + result.Q1_Result + "',Q2 = '" + result.Q2_Result + "',Q3 = '" + result.Q3_Result + "',Q4 = '" + result.Q4_Result + "' where Name = '" + result.UserName + "'");
            }
            else
            {
                SQL.NonScalarQuery("Insert into Judgement (Name                     ,Q1                        ,Q2                        ,Q3                        ,Q4                         ,date)" +
                                                 " VALUES ('" + result.UserName + "','" + result.Q1_Result + "','" + result.Q2_Result + "','" + result.Q3_Result + "','" + result.Q4_Result + "' , (select CONVERT(datetime, '" + dateTime.ToString("yyyy/MM/dd") + "', 20)))");
            }
            return View("~/Views/Judgement/ResponseSubmitted.cshtml");
        }
        public ActionResult Update(Data parameter)
        {
            SQL.ScalarQuery("Update CreateProblem  set p1 = '" + parameter.Q1_P1 + "' where QuestionNo = 1");
            SQL.ScalarQuery("Update CreateProblem  set p2 = '" + parameter.Q1_P2 + "' where QuestionNo = 1");
            SQL.ScalarQuery("Update CreateProblem  set p3 = '" + parameter.Q1_P3 + "' where QuestionNo = 1");
            SQL.ScalarQuery("Update CreateProblem  set p1 = '" + parameter.Q2_P1 + "' where QuestionNo = 2");
            SQL.ScalarQuery("Update CreateProblem  set p2 = '" + parameter.Q2_P2 + "' where QuestionNo = 2");
            SQL.ScalarQuery("Update CreateProblem  set p3 = '" + parameter.Q2_P3 + "' where QuestionNo = 2");
            SQL.ScalarQuery("Update CreateProblem  set p4 = '" + parameter.Q2_P4 + "' where QuestionNo = 2");
            SQL.ScalarQuery("Update CreateProblem  set p1 = '" + parameter.Q3_P1 + "' where QuestionNo = 3");
            SQL.ScalarQuery("Update CreateProblem  set p1 = '" + parameter.Q4_P1 + "' where QuestionNo = 4");
            SQL.ScalarQuery("Update CreateProblem  set p2 = '" + parameter.Q4_P2 + "' where QuestionNo = 4");
            return View("~/Views/Judgement/Success.cshtml");
        }
        public ActionResult LeaderboardDetail()
        {
            string Name = Request.QueryString["Name"];
            var leaderboard = new List<Leaderboard>();

            var results = Main.GetDataTable("select Name,Date,IsNull(TotalCorrect,0) as TotalCorrect  from Judgement where Name = '" + Name.ToString() + "'");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                row.Date = Convert.ToDateTime(Item["Date"]).ToShortDateString();

                row.TotalCorrect = int.Parse(Item["TotalCorrect"].ToString());
                int totalCor = row.TotalCorrect;


                double winPer = 0;
                winPer = (row.TotalCorrect  * 100) / 4   ;
                row.WinPercentage = winPer.ToString();

                leaderboard.Add(row);
            }

            return View(leaderboard);
        }
    }
}