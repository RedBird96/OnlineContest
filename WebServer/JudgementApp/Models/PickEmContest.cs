using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class PickEmContest
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ContestId { get; set; }
        public string CompanyName { get; set; }
        public string ContestName { get; set; }
        public string Logo { get; set; }
        public int StyleType { get; set; }
        public int StockNumber { get; set; }
        public int ExpirationType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DollarsPerPoint { get; set; }
        public int MaxDollars { get; set; }
        public bool IsPublished { get; set; }

        public List<Stock> Stocks = new List<Stock>();

        public bool IsAllowNewContest { get; set; }
    }
}