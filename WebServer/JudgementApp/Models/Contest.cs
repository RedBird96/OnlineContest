using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class Contest
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LogoPath { get; set; }
        public string ContestName { get; set; }
        public int ContestType { get; set; }

    }
}