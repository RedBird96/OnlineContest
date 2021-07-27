using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class Data
    {
        public int Id { get; set; }
        public int? Row_Num { get; set; }
        public int? Type { get; set; }
        public string SymbolName { get; set; }
        public Dictionary<string, string> SymbolList { get; set; }
        public string Title { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
       


    }
}