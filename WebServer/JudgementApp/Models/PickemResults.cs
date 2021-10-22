using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class PickemResults
    {
        public List<PickEmResult> List = new List<PickEmResult>();
    }

    public class PickEmResult
    {
        public long Id { get; set; }
        public long Contestid { get; set; }
        public long Ranking { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public DateTime Date { get; set; }
        public List<string> SubData = new List<string>();
    }
}