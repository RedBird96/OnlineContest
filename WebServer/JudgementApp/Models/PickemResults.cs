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
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}