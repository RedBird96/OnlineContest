using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class StreakJudgment
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Stock { get; set; }
        public string Value { get; set; }
        public long CompanyId { get; set; }
        public long ContestId { get; set; }
        public long QuestionId { get; set; }

    }
}