using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JudgementApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "create-contest-admin",
                url: "create-contest-admin",
                defaults: new { controller = "Judgement", action = "ContestAdmin" }

            );

            routes.MapRoute(
     name: "contest-admin",
     url: "contest-admin/{id}/{ProblemName}",
     defaults: new { controller = "Judgement", action = "CreateProblem", id = UrlParameter.Optional, ProblemName = UrlParameter.Optional }

  );
   routes.MapRoute(
    name: "contest",
    url: "contest-{companyName}-{contestName}",
    defaults: new { controller = "Judgement", action = "Index" }

   
 );
   routes.MapRoute(
   name: "leaderboard",
   url: "leaderboard-{companyName}-{contestName}",
   defaults: new { controller = "Judgement", action = "Leaderboard" }

);

   routes.MapRoute(
       name: "contest-pickem",
       url: "contest-pickem-{companyName}-{contestName}/{id}",
       defaults: new {controller = "Judgement", action = "PickEmContest" }
   );

   routes.MapRoute(
       name: "contest-streak",
       url: "contest-streak-{companyName}-{contestName}/{id}",
       defaults: new { controller = "Judgement", action = "StreakContest" }
   );

            routes.MapRoute(
       name: "contest-pickem-judgement",
       url: "pickem-judgement-{companyName}-{contestName}/{id}",
       defaults: new { controller = "Judgement", action = "PickEmJudgment" }
   );

            routes.MapRoute(
                name: "contest-streak-judgement",
                url: "streak-judgement-{companyName}-{contestName}/{id}",
                defaults: new { controller = "Judgement", action = "StreakJudgment" }
            );
            routes.MapRoute(
                name: "content-list",
                url: "{companyName}-content-list/{id}",
                defaults: new { controller = "Judgement", action = "ContentList" }
            );
            

            routes.MapRoute(
                name: "streak-history",
                url: "streak-history-{companyName}-{contestName}/{id}",
                defaults: new {controller = "Judgement", action = "StreakHistory"});

            routes.MapRoute(
       name: "pickem-leaderboad",
       url: "pickem-leaderboad-{companyName}-{contestName}/{id}",
       defaults: new { controller = "Judgement", action = "PickEmLeadBoard" }
   );


            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new {  controller = "Judgement", action = "ContestAdmin", id = UrlParameter.Optional }
           );
        }
       
    }
}
