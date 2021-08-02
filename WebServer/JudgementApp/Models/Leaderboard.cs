using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class Leaderboard
    {
        public int Id { get; set; }
        public long FKCompany { get; set; }
        public string ProblemName { get; set; }
        public string Username { get; set; }
        public string UserEmail { get; set; }
        

        public int ContestAttempted { get; set; }

        public int TotalCorrect { get; set; }

        public string WinPercentage { get; set; }

        public string Date { get; set; }

        public int TotalQuestionCount { get; set; }

    }
}