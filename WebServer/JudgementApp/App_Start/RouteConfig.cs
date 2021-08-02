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
     name: "contest-admin",
     url: "contest-admin/{id}",
     defaults: new { controller = "Judgement", action = "CreateProblem" }

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
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Judgement", action = "Leaderboard", id = UrlParameter.Optional }
           );
        }
       
    }
}
