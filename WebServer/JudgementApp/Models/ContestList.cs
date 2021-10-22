using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace JudgementApp.Models
{
    public class ContestList
    {
        public List<ContestInfo> List = new List<ContestInfo>();
    }

    public class ContestInfo
    {
        public long DeleteId { get; set; }
        public String Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}