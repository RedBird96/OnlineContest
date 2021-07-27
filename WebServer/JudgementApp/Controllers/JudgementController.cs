using JudgementApp.Models;
using Newtonsoft.Json;
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
            return View(GetData());
        }
        #region"Create problem"
        public List<Data> GetData()
        {
            DataTable dt = Main.GetDataTable("prc_GetProblem");
            //To take the symbol from the website comment the below code 
            var model = new List<Data>();
            string[] symbolNames = { "SPY", "QQQ", "IWM", "TLT", "TSLA", "NFLX", "AAPL", "AMZN", "FB", "GOOGL", "NVDA" };
            int id = 0;
            ViewData["symbolNames"] = symbolNames;
            var symbolNameDic = new Dictionary<string, string>();
            symbolNameDic.Add("", "--Select--");

            foreach (var line in symbolNames)
            {
                symbolNameDic.Add(line, line);

            }
            foreach (DataRow dr in dt.Rows)
            {
                Data data = new Data();
                data.Id = Convert.ToInt32(dr["Id"]);
                data.P1 = dr["P1"].ToString();
                data.P2 = dr["P2"].ToString();
                data.P3 = dr["P3"].ToString();
                data.P4 = dr["P4"].ToString();
                data.Row_Num = Convert.ToInt32(dr["Row_Num"]);
                data.SymbolList = symbolNameDic;
                data.Title = dr["Title"].ToString();
                data.Type = Convert.ToInt32(dr["Type"]);
                model.Add(data);
            }
            return model;

           

        }
        public JsonResult LoadData()
        {
            return Json(GetData(), JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult CreateProblem()
        {
         
            return View(GetData());

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
        public string AddQuestion(string input = "")
        {
            input = Request.Form["data"];

            Data model = new Data();



            model = JsonConvert.DeserializeObject<Data>(input);
            SQL.NonScalarQuery("exec prc_AddQuestion '" + model.Title + "'," + model.Type );

            return "save";
        }
        public string RemoveQuestion(string input = "")
        {
            input = Request.Form["data"];

            Data model = new Data();



            model = JsonConvert.DeserializeObject<Data>(input);
            SQL.NonScalarQuery("Delete from Questions where PKQuestion=" + model.Id);
            SQL.NonScalarQuery("Delete from CreateProblem where QuestionNo=" + model.Id);
            return "Delte";
        }
        public ActionResult Update(string input = "")
        {
            input = Request.Form["data"];

            List<Data> model = new List<Data>();

            model = JsonConvert.DeserializeObject<List<Data>>(input);

            foreach (Data parameter in model)
            {
                SQL.NonScalarQuery("Update CreateProblem  set p1 = '" + parameter.P1 + "' where QuestionNo =" + parameter.Id);
                SQL.NonScalarQuery("Update CreateProblem  set p2 = '" + parameter.P2 + "' where QuestionNo =" + parameter.Id);
                SQL.NonScalarQuery("Update CreateProblem  set p3 = '" + parameter.P3 + "' where QuestionNo =" + parameter.Id);
                SQL.NonScalarQuery("Update CreateProblem  set p4 = '" + parameter.P4 + "' where QuestionNo =" + parameter.Id);

            }
            return View("~/Views/Judgement/Success.cshtml");
        }

        #endregion
        public ActionResult Leaderboard()
        {
            var leaderboard = new List<Leaderboard>();

            var results = Main.GetDataTable("select Name  from Judgement group by Name");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                int totalCorrect = 0;
                int totalQuestionCount = 0;
                int.TryParse(SQL.ScalarQuery("select SUM(TotalCorrect) from Judgement where name = '" + row.Username + "'"), out totalCorrect);
                row.TotalCorrect = totalCorrect;
                int.TryParse(SQL.ScalarQuery("select SUM(ProblemNo) from Judgement where name = '" + row.Username + "'"), out totalQuestionCount);
                row.ContestAttempted = Convert.ToInt32(SQL.ScalarQuery("select Count(*) from Judgement where Name = '" + row.Username + "'"));
                double winPer = 0;
                if (totalQuestionCount != 0)
                    winPer = (row.TotalCorrect *100) / (totalQuestionCount);
                row.WinPercentage = winPer.ToString();

                leaderboard.Add(row);
            }

            return View(leaderboard);
        }
        public ActionResult saveJudgement(JudgementParameter result)
        {
           string input = Request.Form["data"];
            string max = SQL.ScalarQuery("select count(*) from Questions");
            result = JsonConvert.DeserializeObject<JudgementParameter>(input);
            DateTime dateTime = DateTime.UtcNow.Date;
            if (Main.CheckUser(result.UserName))
            {
                string query = "update Judgement set ";
                var i = 0;
                    foreach (JudgmentQ judgment in result.Result)
                {
                    if (i > 0)
                    {
                        query += ",";
                    }
                    i++;
                    query += "Q"+ judgment.ID+" = '" + judgment.QResult + "',P"+ judgment.ID+" = '" + judgment.Param + "',T" + judgment.ID + " = '" + judgment.QType + "'";
                }
                query += " where Name = '" + result.UserName + "'";
                    SQL.NonScalarQuery(query);
            }
            else
            {
                string query = "Insert into Judgement (Name,date,ProblemNo";
                var i = 0;
                foreach (JudgmentQ judgment in result.Result)
                {
                   
                    query += ",Q" + judgment.ID + ",P" + judgment.ID + ",T" + judgment.ID;
                }
                query += ")values('" + result.UserName + "',GetDate(),"+max;
                foreach (JudgmentQ judgment in result.Result)
                {
                   
                    query += ",'" + judgment.QResult + "', '" + judgment.Param + "','" + judgment.QType + "'";
                }
                query += ")";
                
                SQL.NonScalarQuery(query);
            }
            return View("~/Views/Judgement/ResponseSubmitted.cshtml");
        }

       public ActionResult ResponseSubmitted()
        {
            return View();
        }
        public ActionResult LeaderboardDetail()
        {
            string Name = Request.QueryString["Name"];
            var leaderboard = new List<Leaderboard>();

            var results = Main.GetDataTable("select Name,Date,IsNull(TotalCorrect,0) as TotalCorrect, ProblemNo  from Judgement where Name = '" + Name.ToString() + "'");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                row.Date = Convert.ToDateTime(Item["Date"]).ToShortDateString();

                row.TotalCorrect = int.Parse(Item["TotalCorrect"].ToString());
                int totalCor = row.TotalCorrect;

                row.TotalQuestionCount = int.Parse(Item["ProblemNo"].ToString());

                double winPer = 0;
                if (row.TotalQuestionCount != 0)
                    winPer = (row.TotalCorrect  * 100) / row.TotalQuestionCount;
                row.WinPercentage = winPer.ToString();

                leaderboard.Add(row);
            }

            return View(leaderboard);
        }
    }
}