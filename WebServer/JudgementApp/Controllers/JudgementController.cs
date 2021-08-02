using JudgementApp.Common;
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
using System.Web.Routing;

namespace JudgementApp.Controllers
{
    public class JudgementController : Controller
    {
        // GET: Judgement
        public ActionResult Index(string companyName, string contestName)
        {
            try
            {

                ViewData["contestName"] = contestName;
                string id = "";
                id = SQL.ScalarQuery("Select PKCompany from Company where CompanyName='" + companyName + "' and IsActive=1");
                long FKCompany = Convert.ToInt64(id);

                //var model = new Data();
                //model.Q1_P1 = Main.GetParameter(1, "p1");
                return View(GetData(FKCompany, contestName));
            }
            catch(Exception ex)
            {
                Exception exception = new Exception("Internal Server Error");
                throw exception;
            }
        }
        #region"Create problem"
        public List<Data> GetData(long FKCompany,string ProblemName)
        {
            DataTable dt = Main.GetDataTable("prc_GetProblem '"+FKCompany+"','"+ ProblemName + "'");
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
        ViewData["Company"]=    GetCompanyInfo(FKCompany, ProblemName);
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
        public JsonResult LoadData(string id,string ProblemName)
        {
            long FKCompany = Convert.ToInt64(id);
            return Json(GetData(FKCompany, ProblemName), JsonRequestBehavior.AllowGet); ;
        }
        [Route("contest-admin/{id?}/{ProblemName?}")]
        public ActionResult CreateProblem(string id="0",string ProblemName="")
        {
           
            ViewData["contestName"] = ProblemName;

            long FKCompany = Convert.ToInt64(id);
            return View(GetData(FKCompany, ProblemName));

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
            long FKCompany = Convert.ToInt64(model.FKCompany);
            SQL.NonScalarQuery("exec prc_AddQuestion '" + model.Title + "'," + model.Type +","+ FKCompany+ ",'" + model.ProblemName + "'");

            return "save";
        }
        public string RemoveQuestion(string input = "")
        {
            input = Request.Form["data"];

            Data model = new Data();



            model = JsonConvert.DeserializeObject<Data>(input);
            SQL.NonScalarQuery("Delete from Questions where PKQuestion=" + model.Id);
            SQL.NonScalarQuery("Delete from CreateProblem where QuestionNo=" + model.Id +" and ProblemName='"+model.ProblemName+"' and FKCompany="+model.FKCompany);
            return "Delte";
        }
        public ActionResult Update(string input = "")
        {
            input = Request.Form["data"];

            List<Data> model = new List<Data>();

            model = JsonConvert.DeserializeObject<List<Data>>(input);

            foreach (Data parameter in model)
            {
                if (Main.QuestionExists(parameter.ProblemName, parameter.Id, parameter.FKCompany))
                {
                    SQL.NonScalarQuery("Update CreateProblem  set p1 = '" + parameter.P1 + "', p2 = '" + parameter.P2 + "', p3 = '" + parameter.P3 + "', p4 = '" + parameter.P4 + "',ProblemName='" + parameter.ProblemName + "' where QuestionNo =" + parameter.Id);
                                   }
                else
                {
                    SQL.NonScalarQuery("Insert into CreateProblem([FKCompany],[ProblemName],[QuestionNo],[P1],[P2],[P3],[P4]) values("+ parameter .FKCompany+ ",'" + parameter.ProblemName + "','" + parameter.Id + "','" + parameter.P1 + "','" + parameter.P2 + "','" + parameter.P3 + "' ,'" + parameter.P4 + "')" );
                  
                }
            }
            return View("~/Views/Judgement/Success.cshtml");
        }

        #endregion
        public ActionResult Leaderboard(string companyName, string contestName)
        {
            var leaderboard = new List<Leaderboard>();
            ViewData["contestName"] = contestName;
            string id = "";
            id = SQL.ScalarQuery("Select PKCompany from Company where CompanyName='" + companyName + "' and IsActive=1");
            long FKCompany = Convert.ToInt64(id);
            ViewData["Company"] = GetCompanyInfo(FKCompany, contestName);
            var results = Main.GetDataTable("select Name,UserEmail  from Judgement where ProblemName='" + contestName+"' and FKCompany='"+FKCompany+"' group by Name");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                row.UserEmail = Item["UserEmail"].ToString();
                
                int totalCorrect = 0;
                int totalQuestionCount = 0;
                int.TryParse(SQL.ScalarQuery("select SUM(TotalCorrect) from Judgement where name = '" + row.Username + "' and UserEmail='"+row.UserEmail+"' and ProblemName='" + contestName + "' and FKCompany='" + FKCompany + "'"), out totalCorrect);
                row.TotalCorrect = totalCorrect;
                int.TryParse(SQL.ScalarQuery("select SUM(ProblemNo) from Judgement where name = '" + row.Username + "'  and UserEmail='" + row.UserEmail + "' and ProblemName='" + contestName + "' and FKCompany='" + FKCompany + "'"), out totalQuestionCount);
                row.ContestAttempted = Convert.ToInt32(SQL.ScalarQuery("select Count(*) from Judgement where Name = '" + row.Username + "'  and UserEmail='" + row.UserEmail + "' and ProblemName='" + contestName + "' and FKCompany='" + FKCompany + "'"));
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
            
            result = JsonConvert.DeserializeObject<JudgementParameter>(input);
            string max = SQL.ScalarQuery("select isnull(count(*),0)+4 from Questions where ProblemName='"+ result.ProblemName + "' and FKCompany='"+result.FKCompany+"'");
            DateTime dateTime = DateTime.UtcNow.Date;
            long FKCompany = Convert.ToInt64(result.FKCompany);


            if (Main.CheckUser(result.UserName,result.ProblemName, FKCompany, result.UserEmail))
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
                query += " where Name = '" + result.UserName + "' and UserEmail='"+result.UserEmail+"' and ProblemName = '" + result.ProblemName + "' and FKCompany=" + FKCompany;
                    SQL.NonScalarQuery(query);
            }
            else
            {
                string query = "Insert into Judgement (Name,UserEmail,date,ProblemNo,FKCompany,ProblemName";
                var i = 0;
                foreach (JudgmentQ judgment in result.Result)
                {
                   
                    query += ",Q" + judgment.ID + ",P" + judgment.ID + ",T" + judgment.ID;
                }
                query += ")values('" + result.UserName + "','"+result.UserEmail+"',GetDate(),"+max+ ","+ FKCompany + ",'"+result.ProblemName+"'";
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
            string UserEmail = Request.QueryString["UserEmail"];
            string contestName = Request.QueryString["contestName"];
            long FKCompany =Convert.ToInt64( Request.QueryString["FKCompany"]);
            ViewData["Company"] = GetCompanyInfo(FKCompany, contestName);
            var leaderboard = new List<Leaderboard>();

            var results = Main.GetDataTable("select Name,UserEmail,Date,IsNull(TotalCorrect,0) as TotalCorrect, ProblemNo  from Judgement where Name = '" + Name.ToString() + "' and UserEmail='"+UserEmail+"' and ProblemName='" + contestName + "' and FKCompany='" + FKCompany + "'");

            foreach (DataRow Item in results.Rows)
            {
                var row = new Leaderboard();
                row.Username = Item["Name"].ToString();
                row.UserEmail = Item["UserEmail"].ToString();
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

       public Company GetCompanyInfo(long PKCompany,string problemName)
        {
            Company company = new Company();
            DataTable dt = Main.GetDataTable("prc_GetCompany '"+PKCompany+"'");
            if(dt!=null&&dt.Rows.Count>0)
            foreach (DataRow Item in dt.Rows)
            {
                    company.PKCompany = Convert.ToInt64(Item["PKCompany"]);
                    company.CompanyName = Convert.ToString(Item["CompanyName"]);
                    if (problemName != null && problemName.Length > 0)
                    {
                        ViewData["contestName"] = problemName;
                        company.ProblemName = problemName;
                    }
                    else
                    {
                        company.ProblemName = Convert.ToString(Item["ProblemName"]);

                    }

                    company.Logo= Convert.ToString(Item["Logo"]);
                    if ((!DBNull.Value.Equals(Item["BackgroundColor"])) && Item["BackgroundColor"].ToString().Length > 0)
                    {
                        company.BackgroundColor = Convert.ToString(Item["BackgroundColor"]);
                    }
                    else
                    {
                        company.BackgroundColor = "rgb(24,31,42)";
                    }
                    if ((!DBNull.Value.Equals(Item["CardBackgroundColor"])) && Item["CardBackgroundColor"].ToString().Length > 0)
                    {
                        company.CardBackgroundColor = Convert.ToString(Item["CardBackgroundColor"]);
                    }
                    else
                    {
                        company.CardBackgroundColor = "rgb(17,23,31)";
                    }
                    if ((!DBNull.Value.Equals(Item["HeadingColor"])) && Item["HeadingColor"].ToString().Length > 0)
                    {
                        company.HeadingColor = Convert.ToString(Item["HeadingColor"]);
                    }
                    else
                    {
                        company.HeadingColor = "rgb(255,255,255)";
                    }
                    if ((!DBNull.Value.Equals(Item["TableHeaderColor"])) && Item["TableHeaderColor"].ToString().Length > 0)
                    {
                        company.TableHeaderColor = Convert.ToString(Item["TableHeaderColor"]);
                    }
                    else
                    {
                        company.TableHeaderColor = "rgb(16,173,137)";
                    }
                    if ((!DBNull.Value.Equals(Item["TextColor"])) && Item["TextColor"].ToString().Length > 0)
                    {
                        company.TextColor = Convert.ToString(Item["TextColor"]);
                    }
                    else
                    {
                        company.TextColor = "rgb(255,255,255)";
                    }
                    if ((!DBNull.Value.Equals(Item["LinkColor"])) && Item["LinkColor"].ToString().Length > 0)
                    {
                        company.LinkColor = Convert.ToString(Item["LinkColor"]);
                    }
                    else
                    {
                        company.LinkColor = "rgb(133, 135, 150)";
                    }

                }
                return company;
        }
        public JsonResult SearchCompany(long id,string prob=null)
        {
            long FKCompany = Convert.ToInt64(id);
            return Json(GetCompanyInfo( id, prob), JsonRequestBehavior.AllowGet); ;
        }
    }
}