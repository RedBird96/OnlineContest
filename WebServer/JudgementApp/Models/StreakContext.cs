using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class StreakContext
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ContestId { get; set; }
        public string CompanyName { get; set; }
        public string ContestName { get; set; }
        public string Logo { get; set; }
        public DateTime? EndDate { get; set; }
        public string SelectedStocks { get; set; }
        public bool IsPublished { get; set; }

        public List<Stock> Stocks = new List<Stock>();

    }
}