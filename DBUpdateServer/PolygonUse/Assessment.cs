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
        private double rate { get; set; }

        public Interval(DateTime timestamp, double rate) 
        {
            this.timestamp = timestamp;
            this.rate = rate;
        }

        public DateTime GetTimestamp() 
        {
            return timestamp;
        }

        public double GetRate() 
        {
            return rate;
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

        private void LoadHistoryBar(string symbol, int days)
        {
            this.symbol = symbol;
            intervals.Clear();

            string unit = "minute";

            //You have to set here yesterday date
            DateTime startDT = DateTime.UtcNow.AddDays(-1 * days);

            Configuration config = new Configuration();
            config.AddApiKey("apiKey", AccessToken);
            StocksEquitiesApi stocksApi = new StocksEquitiesApi(config);

            var resp = stocksApi.V2AggsTickerStocksTickerRangeMultiplierTimespanFromToGet(symbol, 30, unit,
                            startDT.ToString("yyyy-MM-dd"), DateTime.UtcNow.ToString("yyyy-MM-dd"));

            if (resp.Status == "OK" && resp.ResultsCount > 0)
            {
                foreach (var item in resp.Results)
                {
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(item.T / 1000));
                    intervals.Add(new Interval(dateTimeOffset.DateTime, Convert.ToDouble(item.C)));
                }

            }
        }

        public string AssessQ1(string symbol, double rate, DateTime dateTime)
        {
            LoadHistoryBar(symbol, 1);

            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().TimeOfDay == dateTime.TimeOfDay)
                {
                    if (interval.GetRate() > rate) return "Fact";
                    else return "Fiction";
                }
            }
            //Interval not found (maybe symbol is not correct).
            return "Fiction";
        }

        //isAbove = True (Above)
        //isAbove = False (Below)
        public string AssessQ2(string symbol, bool isAbove, double rate, int days) 
        {
            LoadHistoryBar(symbol, days);

            if (isAbove)
            {
                foreach (Interval interval in intervals)
                {
                    if (interval.GetRate() > rate) return "Fact";
                }
            }

            else 
            {
                foreach (Interval interval in intervals)
                {
                    if (interval.GetRate() < rate) return "Fact";
                }
            }

            return "Fiction";
        }

        public string AssessQ3(string symbol)
        {
            LoadHistoryBar(symbol, 1);

            double rate_9_30 = 0, rate_12_00 = 0, rate_16_00 = 0;

            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour == 9 && interval.GetTimestamp().Minute == 30) rate_9_30 = interval.GetRate();
                if (interval.GetTimestamp().Hour == 12 && interval.GetTimestamp().Minute == 0) rate_12_00 = interval.GetRate();
                if (interval.GetTimestamp().Hour == 16 && interval.GetTimestamp().Minute == 0) rate_16_00 = interval.GetRate();
            }

            double AM = rate_12_00 - rate_9_30;
            double PM = rate_16_00 - rate_12_00;

            if (AM > PM) return "AM";
            else return "PM";

        }

        public string AssessQ4(string symbol, DateTime dateTime)
        {
            LoadHistoryBar(symbol, 1);

            double rate_9_30 = 0, rate_t = 0, rate_16_00 = 0;

            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour == 9 && interval.GetTimestamp().Minute == 30) rate_9_30 = interval.GetRate();
                if (interval.GetTimestamp().Hour == dateTime.TimeOfDay.Hours && interval.GetTimestamp().Minute == dateTime.TimeOfDay.Minutes) rate_t = interval.GetRate();
                if (interval.GetTimestamp().Hour == 16 && interval.GetTimestamp().Minute == 0) rate_16_00 = interval.GetRate();
            }

            double after = rate_t - rate_9_30;
            double before = rate_16_00 - rate_t;

            if (before > after) return "Before";
            else return "After";

        }
    }
}
