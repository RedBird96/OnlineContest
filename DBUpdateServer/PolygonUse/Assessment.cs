using PolygonIO.Api;
using PolygonIO.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonUse
{
    class Interval 
    {
        private DateTime timestamp { get; set; }
        private double Open { get; set; }
        private double High { get; set; }
        private double Low { get; set; }
        private double Close { get; set; }


        public Interval(DateTime timestamp, double open, double high, double low, double close) 
        {
            this.timestamp = timestamp;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
        }

        public DateTime GetTimestamp() 
        {
            return timestamp;
        }

        public double GetOpen()
        {
            return Open;
        }
        public double GetHigh()
        {
            return High;
        }
        public double GetLow()
        {
            return Low;
        }
        public double GetClose() 
        {
            return Close;
        }
    }

    class Assessment
    {
        private readonly string AccessToken = "K6CD972EJqZjLSQ6UNjRIYu78e8_tSLK";
        private string symbol { get; set; }
        private List<Interval> intervals { get; set; }

        public Assessment()
        {
            intervals = new List<Interval>();
        }

        private void LoadHistoryBar(string symbol, DateTime startDT, int nuit, string unit_string)
        {
            this.symbol = symbol;
            intervals.Clear();

            //You have to set here yesterday date
            DateTime endDT = DateTime.Now;

            Configuration config = new Configuration();
            config.AddApiKey("apiKey", AccessToken);
            StocksEquitiesApi stocksApi = new StocksEquitiesApi(config);

            var resp = stocksApi.V2AggsTickerStocksTickerRangeMultiplierTimespanFromToGet(symbol, nuit, unit_string,
                            startDT.ToString("yyyy-MM-dd"), endDT.ToString("yyyy-MM-dd"));

            if (resp.Status == "OK" && resp.ResultsCount > 0)
            {
                foreach (var item in resp.Results)
                {
                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds((double)item.T / 1000).ToLocalTime();
                    intervals.Add(new Interval(dtDateTime, Convert.ToDouble(item.O), Convert.ToDouble(item.H), Convert.ToDouble(item.L), Convert.ToDouble(item.C)));
                }
            }
        }

        public string AssessQ1(string symbol, double rate, DateTime dateTime, DateTime historyDateTime)
        {
            LoadHistoryBar(symbol, historyDateTime, 30, "minute");

            double high_price = 0;
            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour < 9 || interval.GetTimestamp().Hour > 16)
                    continue;

                if (interval.GetTimestamp().TimeOfDay == dateTime.TimeOfDay)
                {
                    if (high_price > rate) return "Fact";
                    else return "Fiction";
                }
                if (interval.GetHigh() > high_price)
                    high_price = interval.GetHigh();
            }
            //Interval not found (maybe symbol is not correct).
            return "Fiction";
        }

        //isAbove = True (Above)
        //isAbove = False (Below)
        public string AssessQ2(string symbol, bool isAbove, int days, double rate, DateTime historyDateTime) 
        {
            LoadHistoryBar(symbol, historyDateTime, days, "day");

            if (isAbove)
            {
                foreach (Interval interval in intervals)
                {
                    if (historyDateTime.Day == interval.GetTimestamp().Day)
                        if (interval.GetClose() > rate) return "Fact";
                }
            }

            else 
            {
                foreach (Interval interval in intervals)
                {
                    if (historyDateTime.Day == interval.GetTimestamp().Day)
                        if (interval.GetClose() < rate) return "Fact";
                }
            }
            
            return "Fiction";
        }

        public string AssessQ3(string symbol, DateTime historyDateTime)
        {
            LoadHistoryBar(symbol, historyDateTime, 30, "minute");

            double close_1530 = 0, close_1130 = 0;
            double open_0930 = 0, open_1200 = 0;

            double am_val = 0;
            double pm_val = 0;

            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour == 9 && interval.GetTimestamp().Minute == 30)
                    open_0930 = interval.GetOpen();
                else if (interval.GetTimestamp().Hour == 11 && interval.GetTimestamp().Minute == 30)
                    close_1130 = interval.GetClose();
                else if (interval.GetTimestamp().Hour == 12 && interval.GetTimestamp().Minute == 00)
                    open_1200 = interval.GetOpen();
                else if (interval.GetTimestamp().Hour == 15 && interval.GetTimestamp().Minute == 30)
                    close_1530 = interval.GetClose();
            }

            am_val = ((close_1130 - open_0930) / open_0930) * 100;
            pm_val = ((close_1530 - open_1200) / open_1200) * 100; 

            if (am_val > pm_val) return "AM";
            else return "PM";

        }

        public string AssessQ4(string symbol, DateTime dateTime, DateTime historyDateTime)
        {
            LoadHistoryBar(symbol, historyDateTime, 30, "minute");

            DateTime pre_30min = dateTime.AddMinutes(-30);

            double close_1530 = 0, close_predt = 0;
            double open_0930 = 0, open_dt = 0;

            double before_val = 0;
            double after_val = 0;
            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour == 9 && interval.GetTimestamp().Minute == 30)
                    open_0930 = interval.GetOpen();
                else if (interval.GetTimestamp().Hour == pre_30min.Hour && interval.GetTimestamp().Minute == pre_30min.Minute)
                    close_predt = interval.GetClose();
                else if (interval.GetTimestamp().Hour == dateTime.Hour && interval.GetTimestamp().Minute == dateTime.Minute)
                    open_dt = interval.GetOpen();
                else if (interval.GetTimestamp().Hour == 15 && interval.GetTimestamp().Minute == 30)
                    close_1530 = interval.GetClose();
            }

            before_val = ((close_predt - open_0930) / open_0930) * 100;
            after_val = ((close_1530 - open_dt) / open_dt) * 100;

            if (before_val > after_val) return "Before";
            else return "After";
        }
    }
}
