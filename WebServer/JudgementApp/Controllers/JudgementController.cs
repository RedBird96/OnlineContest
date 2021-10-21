using JudgementApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            string tempv = ProblemName;
            if (tempv == "")
            {
                tempv = "-1";
            }
            DataTable dt = Main.GetDataTable("prc_GetProblem '"+FKCompany+"','"+ tempv + "'");
            //To take the symbol from the website comment the below code 
            var model = new List<Data>();
            string[] symbolNames = { "SPY", "QQQ", "IWM", "TLT", "TSLA", "NFLX", "AAPL", "AMZN", "FB", "GOOGL", "NVDA" };
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
                if (!DBNull.Value.Equals(dr["IsPublish"]))
                {
                    data.IsPublish = Convert.ToBoolean(dr["IsPublish"]);
                }
                if (!DBNull.Value.Equals(dr["IsExpired"]))
                {
                    data.IsExpired = Convert.ToBoolean(dr["IsExpired"]);
                }
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
            return Json(GetData(FKCompany, ProblemName), JsonRequestBehavior.AllowGet);
        }

        public string ResetContest(string input = "")
        {
            input = Request.Form["data"];
            List<Data> model = new List<Data>();

            model = JsonConvert.DeserializeObject<List<Data>>(input);

            long FKCompany = Convert.ToInt64(model.ElementAt(0).FKCompany);

            SQL.NonScalarQuery("DELETE FROM [dbo].[CreateProblem] WHERE FKCompany=" + FKCompany + " and ProblemName='" + model.ElementAt(0).ProblemName + "'");

            SQL.NonScalarQuery("INSERT into [dbo].[CreateProblem] (FKCompany, ProblemName, QuestionNo,  P1, P2, P3, P4, IsPublish, CreatedDate, IsExpired) values (" + FKCompany + ",'" + model.ElementAt(0).ProblemName + "', 1, '', '', '', '', 'False', GetDate(), 'False')");
            SQL.NonScalarQuery("INSERT into [dbo].[CreateProblem] (FKCompany, ProblemName, QuestionNo,  P1, P2, P3, P4, IsPublish, CreatedDate, IsExpired) values (" + FKCompany + ",'" + model.ElementAt(0).ProblemName + "', 2, '', '', '', '', 'False', GetDate(), 'False')");
            SQL.NonScalarQuery("INSERT into [dbo].[CreateProblem] (FKCompany, ProblemName, QuestionNo,  P1, P2, P3, P4, IsPublish, CreatedDate, IsExpired) values (" + FKCompany + ",'" + model.ElementAt(0).ProblemName + "', 3, '', '', '', '', 'False', GetDate(), 'False')");
            SQL.NonScalarQuery("INSERT into [dbo].[CreateProblem] (FKCompany, ProblemName, QuestionNo,  P1, P2, P3, P4, IsPublish, CreatedDate, IsExpired) values (" + FKCompany + ",'" + model.ElementAt(0).ProblemName + "', 4, '', '', '', '', 'False', GetDate(), 'False')");
            SQL.NonScalarQuery("INSERT into [dbo].[CreateProblem] (FKCompany, ProblemName, QuestionNo,  P1, P2, P3, P4, IsPublish, CreatedDate, IsExpired) values (" + FKCompany + ",'" + model.ElementAt(0).ProblemName + "', 5, '', '', '', '', 'False', GetDate(), 'False')");

            return "reset";
        }

        [Route("contest-admin/{id?}/{ProblemName?}")]
        public ActionResult CreateProblem(string id="0",string ProblemName="")
        {

            var results = Main.GetDataTable("select *  from Contest where Id = " + Convert.ToInt64(id));

            if (results.Rows.Count > 0)
            {
                
                var row = results.Rows[0];
                ViewData["contestName"] = (string)row["ContestName"];

                ViewBag.companyId = (long) row["CompanyId"];
                ViewBag.contestid = id;
                
                var companyInfo = GetCompanyInfo((long)row["CompanyId"], null);
                ViewBag.companyName = companyInfo.CompanyName;
                return View(GetData((long)row["CompanyId"], ProblemName));
            }
            else
            {
                return RedirectToAction("NotFound");
            }
            

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
            //  SQL.NonScalarQuery("Delete from Questions where PKQuestion=" + model.Id);
            SQL.NonScalarQuery("INSERT INTO [dbo].[CreateProblemArchive](ID,[FKCompany],[ProblemName] ,[QuestionNo]) values( " + model.Id + "," + model.FKCompany + ",'" + model.ProblemName + "'," + model.Id + ") ");
            SQL.NonScalarQuery("Delete from CreateProblem where QuestionNo=" + model.Id +" and ProblemName='"+model.ProblemName+"' and FKCompany="+model.FKCompany);
            return "Delte";
        }
        public ActionResult Update(string input = "", HttpPostedFileBase file =null)
        {
            input = Request.Form["data"];
            List<Data> model = new List<Data>();

            model = JsonConvert.DeserializeObject<List<Data>>(input);

            foreach (Data parameter in model)   
            {
                if (Main.QuestionExists(parameter.ProblemName, parameter.Id, parameter.FKCompany))
                {
                    SQL.NonScalarQuery("Update CreateProblem  set p1 = '" + parameter.P1 + "', p2 = '" + parameter.P2 + "', p3 = '" + parameter.P3 + "', p4 = '" + parameter.P4 + "',ProblemName='" + parameter.ProblemName + "',IsPublish='"+ parameter .IsPublish+ "' where QuestionNo =" + parameter.Id+" and FKCompany='"+ parameter.FKCompany + "' and ProblemName='" + parameter.ProblemName + "'");
                }
                else
                {
                    SQL.NonScalarQuery("Insert into CreateProblem([FKCompany],[ProblemName],[QuestionNo],[P1],[P2],[P3],[P4],IsPublish, CreatedDate, IsExpired) values(" + parameter .FKCompany+ ",'" + parameter.ProblemName + "','" + parameter.Id + "','" + parameter.P1 + "','" + parameter.P2 + "','" + parameter.P3 + "' ,'" + parameter.P4 + "','" + parameter.IsPublish + "',GetDate(),'False'" +")");
                    
                }
            }

            string folderName = "assets/img/company/" + model[0].FKCompany;

            if (file != null)
            {
                List<string> LastFiles = new List<string>();
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~")+ folderName;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = model[0].FKCompany+"_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "__" +  file.FileName.Trim('"');
                string fullPath ="/" +folderName + "/"+fileName;
                file.SaveAs(filePath +"/"+ fileName);
                SQL.NonScalarQuery("Update Company set Logo='" + fullPath + "' where PKCompany=" + model[0].FKCompany);
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
            var results = Main.GetDataTable("select Name,UserEmail  from Judgement where ProblemName='" + contestName+"' and FKCompany='"+FKCompany+ "' group by Name,UserEmail");

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

        public JsonResult CheckUserEmail(string UserName, string ProblemName, string FKCompanyID)
        {
            long FKCompany = Convert.ToInt64(FKCompanyID);
            bool res = Main.CheckUser(UserName, ProblemName, FKCompany);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveJudgement(JudgementParameter result)
        {
           string input = Request.Form["data"];
            
            result = JsonConvert.DeserializeObject<JudgementParameter>(input);
            string max = SQL.ScalarQuery("select isnull(count(*),0)+ 5 from Questions where ProblemName='"+ result.ProblemName + "' and FKCompany='"+result.FKCompany+"'");
            DateTime dateTime = DateTime.UtcNow.Date;
            long FKCompany = Convert.ToInt64(result.FKCompany);

            //if (Main.CheckUser(result.UserName,result.ProblemName, FKCompany, result.UserEmail))
            {
                string del_query = "delete from Judgement where Name = '" + result.UserName + "' and UserEmail='" + result.UserEmail + "' and ProblemName = '" + result.ProblemName + "' and FKCompany=" + FKCompany;
                SQL.NonScalarQuery(del_query);
                /*string query = $"update Judgement set ProblemNo = {max}, ";
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
                else*/
                string query = "Insert into Judgement (Name,UserEmail,date,ProblemNo,FKCompany,ProblemName";
                foreach (JudgmentQ judgment in result.Result)
                {
                   
                    query += ",Q" + judgment.ID + ",P" + judgment.ID + ",T" + judgment.ID + ",R" + judgment.ID;
                }
                query += ")values('" + result.UserName + "','"+result.UserEmail+"',GetDate(),"+max+ ","+ FKCompany + ",'"+result.ProblemName+"'";
                foreach (JudgmentQ judgment in result.Result)
                {
                   
                    query += ",'" + judgment.QResult + "', '" + judgment.Param + "','" + judgment.QType + "','False'";
                }
                query += ")";
                
                SQL.NonScalarQuery(query);
            }
            return View("~/Views/Judgement/ResponseSubmitted.cshtml");
        }

        public ActionResult ResponseSubmitted()
        {
          ViewData["contestName"] =  Request.QueryString["ProblemName"];
            ViewData["companyName"] = Request.QueryString["CompanyName"];
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


        #region New Work

        [HttpGet]
        [Route("create-contest-admin")]
        public ActionResult ContestAdmin()
        {
            return View(new Contest());
        }

        [HttpPost]
        public ActionResult CreateContest(Contest contest)
        {
            int insertedVal = 0;
            var results = Main.GetDataTable("select *  from Contest where CompanyId = "+contest.CompanyId+ " and ContestName='" + contest.ContestName + "'");
            if (results.Rows.Count > 0)
            {
                return Json(new
                {
                    isError=true,
                    message="Contest Name with same name already exist for selected company"
                });
            }
            else
            {
                var query = @"
                        Insert Into Contest (companyId,CompanayName,Logo,ContestName,CreatedDate,ContestType,IsPublished)
                        Values("+contest.CompanyId+ ",'" + contest.ContestName + "','" + contest.LogoPath + "','" + contest.ContestName + "',getdate(),"+contest.ContestType+ ",0);SELECT SCOPE_IDENTITY()";
                var val =SQL.ScalarQuery(query);
                insertedVal = int.Parse(val);

                var updateLogoQuery = @" Update Company
                                         set logo = '" + contest.LogoPath + @"'
                                            where id = "+contest.CompanyId;

                SQL.NonScalarQuery(updateLogoQuery);
            }
            return Json(new
            {
                isError = false,
                data = contest.ContestType,
                id = insertedVal
            });
        }


        [HttpPost]
        public ActionResult UploadFile()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    string fname=string.Empty;
                    string path = string.Empty;
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        path = $"{Guid.NewGuid()}-{fname}";
                        fname = Path.Combine(Server.MapPath("~/assets/CompanyLogos"), path);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json(path);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        [Route("contest-pickem-{companyName}-{contestName}/{id}")]
        public ActionResult PickEmContest(int id)
        {
            var pickEmContest = new PickEmContest();
            var results = Main.GetDataTable("select *  from Contest where Id = " + id);
            if (results.Rows.Count > 0)
            {
                var row = results.Rows[0];
                var isPublished = (bool)row["IsPublished"];
                pickEmContest.Logo = (string)row["Logo"];
                pickEmContest.CompanyId = (long)row["CompanyId"];
                pickEmContest.ContestName = (string)row["ContestName"];
                pickEmContest.ContestId = id;
                var companyInfo = GetCompanyInfo(pickEmContest.CompanyId, null);
                pickEmContest.CompanyName = companyInfo.CompanyName;
                if (isPublished)
                {
                    var pickEmContestResult = Main.GetDataTable("select * from PickEmQuestions where contestId  = " + id);
                    var dr = pickEmContestResult.Rows[0];
                    pickEmContest.Id = (long)dr["Id"];
                    pickEmContest.StockNumber = (int)dr["StockNumber"];
                    pickEmContest.ExpirationType = (int)dr["ExpirationType"];
                    pickEmContest.StyleType = (int)dr["Style"];
                    pickEmContest.ExpirationDate = (DateTime)dr["ContestExpiration"];
                    pickEmContest.DollarsPerPoint = dr["TotalDollars"]==DBNull.Value?0: (int)dr["TotalDollars"];
                    pickEmContest.MaxDollars = dr["MaxDollars"] == DBNull.Value ? 0 : (int)dr["MaxDollars"];
                    pickEmContest.IsPublished = true;
                }
            }
            else
            {
                return RedirectToAction("NotFound");
            }

            var request = HttpContext.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            ViewBag.url = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return View(pickEmContest);
        }

        [HttpPost]
        public ActionResult CreateAndPublishPickEmQuestion(PickEmContest model)
        {
           
            DateTime time = DateTime.Now.AddDays(1);
            if (model.ExpirationType == 2)
            {
                time =time.AddDays(7);
            }
            else if(model.ExpirationType==3)
            {
                time = time.AddDays(30);
            }
            else if (model.ExpirationType == 4)
            {
                time = model.ExpirationDate;
            }
            string format = "yyyy-MM-dd HH:mm:ss";
            var finalDate = time.ToString(format);
            if (model.Id > 0)
            {
               
                var query = @"
                        Update PickEmQuestions
                        set StockNumber ="+model.StockNumber+@",ExpirationType="+ model.ExpirationType + @",ContestExpiration='"+finalDate+ @"',Style=" + model.StyleType + @", TotalDollars =" + model.DollarsPerPoint + @",MaxDollars=" + model.MaxDollars + @"
                         where Id =" + model.Id;
                SQL.NonScalarQuery(query);
            }
            else
            {
               
                var query = @"
                         Insert Into PickEmQuestions(CompanyId,ContestId,StockNumber,ExpirationType,ContestExpiration,Style,TotalDollars,MaxDollars,CreatedDate)
                         Values("+model.CompanyId+ "," + model.ContestId + "," + model.StockNumber + "," + model.ExpirationType + ",'"+ finalDate + "'," + model.StyleType + "," + model.DollarsPerPoint + "," + model.MaxDollars + ",getdate());SELECT SCOPE_IDENTITY()";
                var val = SQL.ScalarQuery(query);
               
                var updateQuery = @"Update Contest set IsPublished = 1 where Id =" + model.ContestId;
                SQL.NonScalarQuery(updateQuery);
            }

           
           
            return Json(new
            {
                isError=false,
               
            });
        }

        public ActionResult PickEmJudgment(long id)
        {
            var pickEmContest = new PickEmContest();
            var pickEmContestResult = Main.GetDataTable("select * from PickEmQuestions where id  = " + id);

            if (pickEmContestResult.Rows.Count > 0)
            {
                var dr = pickEmContestResult.Rows[0];
                pickEmContest.Id = (long)dr["Id"];
                pickEmContest.StockNumber = (int)dr["StockNumber"];
                pickEmContest.ExpirationType = (int)dr["ExpirationType"];
                pickEmContest.StyleType = (int)dr["Style"];
                pickEmContest.ExpirationDate = (DateTime)dr["ContestExpiration"];
                pickEmContest.DollarsPerPoint = dr["TotalDollars"] == DBNull.Value ? 0 : (int)dr["TotalDollars"];
                pickEmContest.MaxDollars = dr["MaxDollars"] == DBNull.Value ? 0 : (int)dr["MaxDollars"];
                pickEmContest.IsPublished = true;

                var contestId = (long) dr["ContestId"];
                var results = Main.GetDataTable("select *  from Contest where Id = " + contestId);
                var row = results.Rows[0];
                pickEmContest.Logo = (string)row["Logo"];
                pickEmContest.CompanyId = (long)row["CompanyId"];
                pickEmContest.ContestName = (string)row["ContestName"];
                pickEmContest.ContestId = contestId;
                var companyInfo = GetCompanyInfo(pickEmContest.CompanyId, null);
                pickEmContest.CompanyName = companyInfo.CompanyName;
                pickEmContest.Stocks = GetStocks();

                ViewBag.isExpired = DateTime.Now>pickEmContest.ExpirationDate;
            }
            else
            {
                return RedirectToAction("NotFound");
            }
            
            return View(pickEmContest);
        }

        [HttpPost]
        public ActionResult SavePickEmJudgment(PickEmJudgment model)
        {
            var pickEmContestResult = Main.GetDataTable("select * from PickEmQuestions where contestId  = " + model.ContestId);
            var reUser = Main.GetDataTable("select * from PickEmJudgment where iscalculated=0 and Username = '"+model.Username + "' and contestId =" +model.ContestId);
            var reEmail = Main.GetDataTable("select * from PickEmJudgment where iscalculated=0 and Email = '" + model.Email + "' and contestId =" + model.ContestId);

            var dr = pickEmContestResult.Rows[0];
            var expirayDate = (DateTime)dr["ContestExpiration"];
            if (DateTime.Now > expirayDate)
            {
                return Json(new
                {
                    message = "Contest has been expired",
                    isError = true,

                });
            }

            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);


            //easternTime< Convert.ToDateTime("09:00")
            if (easternTime < Convert.ToDateTime("09:00"))
            {
                if (reUser.Rows.Count > 0)
                {
                    return Json(new
                    {
                        message = "Username already exists. Please select a new one",
                        isError = true,

                    });
                }
                else if (reEmail.Rows.Count > 0)
                {
                    return Json(new
                    {
                        message = "Email already exists. Please select a new one",
                        isError = true,

                    });

                }
                else
                {
                    var query = @"
                         Insert into PickEmJudgment(Username,Email,CompanyId,ContestId,QuestionId,Stocks,Amount,CreatedDate,iscalculated)
                         values('" + model.Username + "','" + model.Email + "'," + model.CompanyId + "," + model.ContestId + "," + model.QuestionId + ",'" + model.Stocks + "','" + model.Amounts + "',getdate(),0)";
                    SQL.NonScalarQuery(query);

                    var query1 = @"                     
Insert into PickEmResults (contestId,Ranking,Username,Score,date)
Values(" + model.ContestId + "," + 0 + ",'" + model.Username + "',0,getdate())";
                    SQL.NonScalarQuery(query1);

                    return Json(new
                    {
                        isError = false,

                    });
                }
            }
            else
            {
                return Json(new
                {
                    isError = true,
                    message = "Results cannot be submitted after 9:00 AM"

                });
            }
           
        }

        public ActionResult PickEmLeadBoard(string id)
        {
            var data = id.Split('-');
            var results = Main.GetDataTable("Select * from PickEmResults where ContestId ="+ data[0]);
            var response = new PickemResults();
            ViewBag.questionId = data[1];
            var pickEmContestResult = Main.GetDataTable("select * from PickEmQuestions where id  = " + data[1]);
            var dr = pickEmContestResult.Rows[0];

            var contestId = (long)dr["ContestId"];
            var results1 = Main.GetDataTable("select *  from Contest where Id = " + contestId);
            var row = results1.Rows[0];
           long companyId = (long)row["CompanyId"];
            ViewBag.ContestName = (string)row["ContestName"];
           
            var companyInfo = GetCompanyInfo(companyId, null);
            ViewBag.CompanyName = companyInfo.CompanyName;
            ViewBag.Logo = companyInfo.Logo;
            foreach (DataRow resultsRow in results.Rows)
            {
                

                var subResults = Main.GetDataTable("select * from PickEmJudgment where contestId = "+ contestId + " and username  = '"+ (string)resultsRow["Username"]+"'");

                var srow =subResults.Rows[0];
                var stocks = (string)srow["stocks"];
                var amount = (string)srow["amount"];

                var stocksData = stocks.Split(',');
                var amountData = string.IsNullOrEmpty(amount)?null: amount.Split(',');
                List<string> subData = new List<string>();
                for (var i=0;i<stocksData.Length;i++)
                {
                    var finishdata = string.Empty;

                    finishdata = amountData==null
                        ? stocksData[i]
                        : $"{stocksData[i]}({amountData[i]})";
                    subData.Add(finishdata);
                }
                response.List.Add(new PickEmResult()
                {
                    Id = (long)resultsRow["Id"],
                    Contestid = (int)resultsRow["Contestid"],
                    Date = (DateTime)resultsRow["Date"],
                    Ranking = (int)resultsRow["Ranking"],
                    Score = (int)resultsRow["Score"],
                    Username = (string)resultsRow["Username"],
                    SubData = subData
                });
            }

            response.List = response.List.OrderBy(o => o.Ranking).ToList();
            return View(response);
        }

        public ActionResult CreateStreakContest(int contestId)
        {
            return View();
        }

        public ActionResult StreakContest(long id)
        {
            var streakContest = new StreakContext();
            var results = Main.GetDataTable("select *  from Contest where Id = " + id);
            if (results.Rows.Count > 0)
            {
                var row = results.Rows[0];
                var isPublished = (bool)row["IsPublished"];
                streakContest.Logo = (string)row["Logo"];
                streakContest.CompanyId = (long)row["CompanyId"];
                streakContest.ContestName = (string)row["ContestName"];
                streakContest.ContestId = id;
                var companyInfo = GetCompanyInfo(streakContest.CompanyId, null);
                streakContest.CompanyName = companyInfo.CompanyName;
                if (isPublished)
                {
                    var pickEmContestResult = Main.GetDataTable("select * from StreakQuestions where contestId  = " + id);
                    var dr = pickEmContestResult.Rows[0];
                    streakContest.Id = (long)dr["Id"];
                    streakContest.EndDate =  dr["EndDate"]==DBNull.Value?null: (DateTime?)dr["EndDate"];
                    streakContest.SelectedStocks = (string) dr["SelectedStocks"];
                    streakContest.IsPublished = true;
                }
            }
            else
            {
                return RedirectToAction("NotFound");
            }

            var request = HttpContext.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            ViewBag.url = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return View(streakContest);
        }

        [HttpPost]
        public ActionResult CreateAndPublishStreakQuestion(StreakContext model)
        {

            
            string format = "yyyy-MM-dd HH:mm:ss";
            var finalDate = model.EndDate!=null? model.EndDate.Value.ToString(format):null;
            if (model.Id > 0)
            {
                var query = string.IsNullOrEmpty(finalDate) ? @"
                         Update StreakQuestions
                        set enddate = null, selectedStocks = '" + model.SelectedStocks + @"'
                        where id =" + model.Id : @"
                         Update StreakQuestions
                        set enddate = '" + finalDate + @"', selectedStocks = '" + model.SelectedStocks + @"'
                        where id =" + model.Id;
                SQL.NonScalarQuery(query);
            }
            else
            {

                var query =string.IsNullOrEmpty(finalDate)?@"Insert Into StreakQuestions(companyId, ContestId, EndDate, SelectedStocks, CreatedDate)
                Values("+model.CompanyId+ ", " + model.ContestId + ", null, '"+model.SelectedStocks+"', getdate()); SELECT SCOPE_IDENTITY()": @"
                    Insert Into StreakQuestions(companyId,ContestId,EndDate,SelectedStocks,CreatedDate)
                    Values("+model.CompanyId+ "," + model.ContestId + ",'" + finalDate + "','"+model.SelectedStocks+"',getdate());SELECT SCOPE_IDENTITY()";
                var val = SQL.ScalarQuery(query);

                var updateQuery = @"Update Contest set IsPublished = 1 where Id =" + model.ContestId;
                SQL.NonScalarQuery(updateQuery);
            }



            return Json(new
            {
                isError = false,

            });
        }

        public ActionResult StreakJudgment(long id)
        {
            var streakContext = new StreakContext();
            var streakContextResult = Main.GetDataTable("select * from StreakQuestions where id  = " + id);

            if (streakContextResult.Rows.Count > 0)
            {
                var dr = streakContextResult.Rows[0];
                streakContext.Id = (long)dr["Id"];
                streakContext.EndDate = dr["EndDate"]==DBNull.Value?null: (DateTime?)dr["EndDate"];
                streakContext.SelectedStocks = (string)dr["SelectedStocks"];
                streakContext.IsPublished = true;

                var contestId = (long)dr["ContestId"];
                var results = Main.GetDataTable("select *  from Contest where Id = " + contestId);
                var row = results.Rows[0];
                streakContext.Logo = (string)row["Logo"];
                streakContext.CompanyId = (long)row["CompanyId"];
                streakContext.ContestName = (string)row["ContestName"];
                streakContext.ContestId = contestId;
                var companyInfo = GetCompanyInfo(streakContext.CompanyId, null);
                streakContext.CompanyName = companyInfo.CompanyName;
                var stocks = GetStocks();
                ViewBag.isExpired = streakContext.EndDate !=null && DateTime.Now > streakContext.EndDate;
                var data = streakContext.SelectedStocks.Split(',');
                if (data[0] != "all")
                {
                    foreach (var s in data)
                    {
                        streakContext.Stocks.AddRange(stocks.Where(o => o.Type.Trim().ToLower() == s.Trim().ToLower()).ToList());
                    }
                }
                else
                {
                    streakContext.Stocks = stocks;
                }


                var weekendDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

                
                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                ViewBag.NextDay = easternTime;
                if (easternTime > Convert.ToDateTime("16:00"))
                {
                    var stockDay = Holiday.GetNextNonHolidayWeekDay(easternTime, new List<Holiday>(), weekendDays);
                    ViewBag.NextDay = stockDay;
                }

                if (easternTime.DayOfWeek == DayOfWeek.Saturday || easternTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    var stockDay = Holiday.GetNextNonHolidayWeekDay(easternTime, new List<Holiday>(), weekendDays);
                    ViewBag.NextDay = stockDay;
                }
                

                
            }
            else
            {
                return RedirectToAction("NotFound");
            }

            return View(streakContext);
        }
        public ActionResult StreakLeadBoard()
        {
            return View();
        }

        private List<Stock> GetStocks()
        {
            var dt = Main.GetDataTable("select * from Stock");
            var stocks = new List<Stock>();
            
            foreach (DataRow row in dt.Rows)
            {
                stocks.Add(new Stock()
                {
                    Name = (string)row["Name"],
                    Type = (string)row["Type"],
                });
            }

            return stocks;
        }

        [HttpPost]
        public ActionResult SaveStreakJudgment(StreakJudgment model)
        {
            var streakContextResult = Main.GetDataTable("select * from StreakQuestions where contestId  = " + model.ContestId);
            var dr = streakContextResult.Rows[0];
            var enddate = dr["EndDate"] == DBNull.Value ? null : (DateTime?)dr["EndDate"];
            if (enddate != null && DateTime.Now > enddate)
            {
                return Json(new
                {
                    message = "Contest has been expired",
                    isError = true,

                });
            }


            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            //easternTime< Convert.ToDateTime("09:00")
            if (easternTime < Convert.ToDateTime("09:00"))
            {
                var reUser = Main.GetDataTable("select * from StreakJudgement where iscalculated=0 and Username = '" + model.Username + "' and contestId =" + model.ContestId );
                var reEmail = Main.GetDataTable("select * from StreakJudgement where iscalculated=0 and Email = '" + model.Email + "' and contestId =" + model.ContestId);

                if (reUser.Rows.Count > 0)
                {
                    return Json(new
                    {
                        message = "Username already exists. Please select a new one.",
                        isError = true,

                    });
                }
                else if (reEmail.Rows.Count > 0)
                {
                    return Json(new
                    {
                        message = "Email already exists. Please select a new one.",
                        isError = true,

                    });

                }
                else
                {
                    var query = @"
                           Insert Into StreakJudgement(username,email,stock,value,companyId,Contestid,QuestionId,CreatedDate,streak,iscalculated)
                        values('" + model.Username + "','" + model.Email + "','" + model.Stock + "','" + model.Value + "'," + model.ContestId + "," + model.ContestId + "," + model.QuestionId + ",getdate(),0,0)";
                    SQL.NonScalarQuery(query);

                    return Json(new
                    {
                        isError = false,

                    });
                }

                
            }
            else
            {
                return Json(new
                {
                    isError = true,
                    message = "Results cannot be submitted after 9:00 AM"

                });
            }
            
        }



        public ActionResult StreakHistory(int id,string email)
        {
           
            ViewBag.questionId = id;
            var pickEmContestResult = Main.GetDataTable("select * from StreakQuestions where id  = " + id);
            var dr = pickEmContestResult.Rows[0];

            var contestId = (long)dr["ContestId"];
            var results1 = Main.GetDataTable("select *  from Contest where Id = " + contestId);
            var row1 = results1.Rows[0];
            long companyId = (long)row1["CompanyId"];
            ViewBag.ContestName = (string)row1["ContestName"];

            var companyInfo = GetCompanyInfo(companyId, null);
            ViewBag.CompanyName = companyInfo.CompanyName;
            ViewBag.Logo = companyInfo.Logo;

            ViewBag.email =email;

            StreakHistory result = new StreakHistory();


            var dt = Main.GetDataTable(@"select createdDate,stock,value,streak
                                                     from StreakJudgement
                                                        where username = '"+ email + "' and contestId="+contestId);
            var stocks = new List<UserHistory>();

            foreach (DataRow row in dt.Rows)
            {
                stocks.Add(new UserHistory()
                {
                    Date = (DateTime)row["createdDate"],
                    Direction = (string)row["value"],
                    Stock = (string)row["Stock"],
                    Streak = (int)row["Streak"]
                });

                
            }

            result.UserHistory = stocks;

            var dt1 = Main.GetDataTable(@"Select t.stock,
  (select count(1) from StreakJudgement where stock = t.stock and value = 'Up' and contestId=" + contestId + ") as UpCount," +
  "(select count(1) from StreakJudgement where stock = t.stock and value = 'Down' and contestId=" + contestId + ") as DownCount " +
  "from (select stock from StreakJudgement where CONVERT(date, createdDate) = CONVERT(date, GETDATE()) and  contestId=" + contestId + " group by stock) as t");
            var today = new List<TodayHistory>();

            foreach (DataRow row in dt1.Rows)
            {
                today.Add(new TodayHistory()
                {
                    Symbol = (string)row["stock"],
                    DownCount = (int)row["DownCount"],
                    UpCount = (int)row["UpCount"],
                    Total = (int)row["DownCount"] + (int)row["UpCount"]
                });
            }

            result.TodayHistory = today;
            return View(result);
        }


        public ActionResult ContentList(long id)
        {
            var companyInfo = GetCompanyInfo(id, null);
            ViewBag.CompanyName = companyInfo.CompanyName;
            var response = new ContestList();
            var result = Main.GetDataTable("select * from Contest where companyid="+id);

            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    var Id = (long) row["Id"];
                    var companyName = (string) row["CompanayName"];
                    var contestName = (string) row["ContestName"];
                    var type = (int) row["ContestType"];
                    var date = (DateTime) row["CreatedDate"];
                   
                    response.List.Add(new ContestInfo()
                    {
                        Name = contestName,
                        Type = type==1?"Custom":type==2?"PickEm":"Streak",
                        CreatedDate = date,
                        Url = type == 1 ? 
                            $"/contest-admin/{Id}/{contestName}" :
                            type == 2 ?
                                $"/contest-pickem-{companyName}-{contestName}/{Id}"
                                : $"/contest-streak-{companyName}-{contestName}/{Id}"
                    });
                  
                       
                    

                }
            }

            return View(response);
        }

        [HttpGet]
        public ActionResult GetUserStreak(string email, long id)
        {
            var result = Main.GetDataTable("Select * from StreakJudgement where contestId = "+ id + " and email = '"+email+"'");

            var strakCount = 0;
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    var count = (int)row["Streak"];

                    strakCount = strakCount + count;
                }
            }

            return Json(strakCount, JsonRequestBehavior.AllowGet);
        }
        #endregion
        }
}