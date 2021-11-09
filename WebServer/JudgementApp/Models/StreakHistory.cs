using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JudgementApp.Models
{
    public class StreakHistory
    {
        public List<UserHistory> UserHistory = new List<UserHistory>();
        public List<TodayHistory> TodayHistory = new List<TodayHistory>();
        public List<TodayHistory> ContentHistory = new List<TodayHistory>();
    }

    public class UserHistory
    {
        public DateTime Date { get; set; }
        public string Stock { get; set; }
        public string Direction { get; set; }
        public int Streak { get; set; }
    }

    public class TodayHistory
    {
        public string Symbol { get; set; }
        public int UpCount { get; set; }
        public int DownCount { get; set; }
        public int Total { get; set; }
    }

    public class StreakResult
    {
        public string Username { get; set; }
        public int Streak { get; set; }
    }

    public class StreakResultList
    {
        public List<StreakResult> List = new List<StreakResult>();
    }
}