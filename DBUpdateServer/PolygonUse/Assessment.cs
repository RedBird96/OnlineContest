using PolygonIO.Api;
using PolygonIO.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonUse
{
    public class Interval
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

    public class Assessment
    {
        private readonly string AccessToken = "K6CD972EJqZjLSQ6UNjRIYu78e8_tSLK";
        private string symbol { get; set; }
        private List<Interval> intervals { get; set; }
        private List<Interval> intervals2 { get; set; }

        public Assessment()
        {
            intervals = new List<Interval>();
            intervals2 = new List<Interval>();
        }

        private void LoadHistoryBar(string symbol, DateTime startDT, int nuit, string unit_string, bool bSecondInterval = false)
        {
            this.symbol = symbol;
            if (!bSecondInterval)
                intervals.Clear();

            intervals2.Clear();

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
                    if (!bSecondInterval)
                        intervals.Add(new Interval(dtDateTime, Convert.ToDouble(item.O), Convert.ToDouble(item.H), Convert.ToDouble(item.L), Convert.ToDouble(item.C)));

                    intervals2.Add(new Interval(dtDateTime, Convert.ToDouble(item.O), Convert.ToDouble(item.H), Convert.ToDouble(item.L), Convert.ToDouble(item.C)));
                }
            }
        }

        public string AssessQ1(string symbol, double rate, DateTime dateTime, DateTime historyDateTime)
        {

            DateTime dtTempEnterDateTime = new DateTime(historyDateTime.Year, historyDateTime.Month, historyDateTime.Day, historyDateTime.Hour, historyDateTime.Minute, 0);

            LoadHistoryBar(symbol, dtTempEnterDateTime, 1, "minute");

            bool IsHigh = true;
            double enter_rate = 0;
            double high_low_price = 0;
            foreach (Interval interval in intervals)
            {
                if (interval.GetTimestamp().Hour > 16)
                    continue;
                if (interval.GetTimestamp() >= dtTempEnterDateTime && enter_rate == 0)
                {
                    enter_rate = interval.GetOpen();
                    if (enter_rate < rate)
                        IsHigh = true;
                    else
                        IsHigh = false;
                }
                if (interval.GetTimestamp() >= dateTime)
                {
                    if (IsHigh)
                    {
                        if (high_low_price > rate) return "Yes";
                        else return "No";
                    }
                    else
                    {
                        if (high_low_price < rate) return "Yes";
                        else return "No";
                    }
                }
                if (IsHigh)
                {
                    if (interval.GetHigh() > high_low_price)
                        high_low_price = interval.GetHigh();
                }
                else
                {
                    if (interval.GetLow() < high_low_price)
                        high_low_price = interval.GetLow();
                }

            }
            //Interval not found (maybe symbol is not correct).
            return "No";
        }

        //isAbove = True (Above)
        //isAbove = False (Below)
        public string AssessQ2(string symbol, bool isAbove, int days, double rate, DateTime historyDateTime)
        {

            if (days == 1)
                LoadHistoryBar(symbol, historyDateTime, 1, "day");
            else if (days == 7)
                LoadHistoryBar(symbol, historyDateTime, 1, "week");
            else if (days == 30)
                LoadHistoryBar(symbol, historyDateTime, 1, "month");

            if (isAbove)
            {
                foreach (Interval interval in intervals)
                {
                    if (historyDateTime.Day == interval.GetTimestamp().Day)
                        if (interval.GetClose() > rate) return "Yes";
                }
            }

            else
            {
                foreach (Interval interval in intervals)
                {
                    if (historyDateTime.Day == interval.GetTimestamp().Day)
                        if (interval.GetClose() < rate) return "Yes";
                }
            }

            return "No";
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

        public string AssessQ5(string symbol1, string symbol2, DateTime dateTime, int days)
        {
            double open_rate1 = 0, close_rate1 = 0;
            double open_rate2 = 0, close_rate2 = 0;

            LoadHistoryBar(symbol1, dateTime, 1, "day");
            LoadHistoryBar(symbol2, dateTime, 1, "day", true);

            foreach (Interval interval in intervals)
                open_rate1 = interval.GetOpen();

            foreach (Interval interval in intervals2)
                open_rate2 = interval.GetOpen();

            if (days == 7)
            {
                LoadHistoryBar(symbol1, dateTime, 1, "week");
                LoadHistoryBar(symbol2, dateTime, 1, "week", true);
            }
            else if (days == 30)
            {
                LoadHistoryBar(symbol1, dateTime, 1, "month");
                LoadHistoryBar(symbol2, dateTime, 1, "month", true);
            }

            foreach (Interval interval in intervals)
                close_rate1 = interval.GetClose();

            foreach (Interval interval in intervals2)
                close_rate2 = interval.GetClose();

            double per1 = (close_rate1 - open_rate1) / open_rate1 * 100;
            double per2 = (close_rate2 - open_rate2) / open_rate2 * 100;

            if (per1 > per2)
                return symbol1;

            return symbol2;
        }

        public double AssessPickA(List<string> symbol, DateTime dtStart, DateTime dtEnd)
        {
            double rise_percent = 0;
            double sum = 0;

            foreach(string symbol_one in symbol)
            { 
                double close_st = 0, close_en = 0;
                LoadHistoryBar(symbol_one, dtStart, 1, "day");
                close_st = intervals.ElementAt(0).GetClose();
                LoadHistoryBar(symbol_one, dtEnd, 1, "day");
                close_en = intervals.ElementAt(0).GetClose();

                rise_percent = (close_en - close_st) / close_st;
                sum += rise_percent;
            }
            return sum;
        }

        public double AssessPickB(List<string> symbol, DateTime dtStart, DateTime dtEnd, List<double> symbol_amount)
        {
            double sum = 0;
            int index = 0;
            List<Tuple<string, double, double, double>> lstRemainSymbol = new List<Tuple<string, double, double, double>>();
            List<Tuple<string, double, double, double>> lstLessSymbol = new List<Tuple<string, double, double, double>>();
            for(index = 0; index < symbol.Count; index ++)
            {
                string symbol_one = symbol[index];
                double open_st, close_en;
                LoadHistoryBar(symbol_one, dtStart, 1, "day");
                open_st = intervals[0].GetOpen();
                LoadHistoryBar(symbol_one, dtEnd, 1, "day");
                close_en = intervals[0].GetClose();
                if (open_st >= symbol_amount[index])
                {
                    lstLessSymbol.Add(Tuple.Create(symbol_one, symbol_amount[index], open_st, close_en));
                }
                else
                {
                    lstRemainSymbol.Add(Tuple.Create(symbol_one, symbol_amount[index], open_st, close_en));
                }
            }

            for(index = 0; index < lstLessSymbol.Count; index ++)
            {
                if (lstLessSymbol[index].Item2 == lstLessSymbol[index].Item3)
                    continue;

                double lack_money = lstLessSymbol[index].Item3 - lstLessSymbol[index].Item2;
                
                for(int indexRemain = 0; indexRemain < lstRemainSymbol.Count; indexRemain ++)
                {
                    double temp_money = lstRemainSymbol[indexRemain].Item2 - lstRemainSymbol[indexRemain].Item3;
                    lack_money = lack_money - temp_money;
                    if (lack_money <= 0)
                    {
                        lstRemainSymbol[indexRemain] = Tuple.Create(lstRemainSymbol[indexRemain].Item1, lstRemainSymbol[indexRemain].Item2 - lack_money, lstRemainSymbol[indexRemain].Item3, lstRemainSymbol[indexRemain].Item4);
                        lstLessSymbol[index] = Tuple.Create(lstLessSymbol[index].Item1, lstLessSymbol[index].Item2 + lack_money, lstLessSymbol[index].Item3, lstLessSymbol[index].Item4);
                        break;
                    }

                    lstRemainSymbol[indexRemain] = Tuple.Create(lstRemainSymbol[indexRemain].Item1, lstRemainSymbol[indexRemain].Item2 - temp_money, lstRemainSymbol[indexRemain].Item3, lstRemainSymbol[indexRemain].Item4);
                    lstLessSymbol[index] = Tuple.Create(lstLessSymbol[index].Item1, lstLessSymbol[index].Item2 + temp_money, lstLessSymbol[index].Item3, lstLessSymbol[index].Item4);
                }

            }

            for (index = 0; index < lstRemainSymbol.Count; index++)
            {
                sum = sum + (int)(lstRemainSymbol[index].Item2 / lstRemainSymbol[index].Item3) * (lstRemainSymbol[index].Item4 - lstRemainSymbol[index].Item3);
            }

            for (index = 0; index < lstLessSymbol.Count; index++)
            {
                sum = sum + (int)(lstLessSymbol[index].Item2 / lstLessSymbol[index].Item3) * (lstLessSymbol[index].Item4 - lstLessSymbol[index].Item3);
            }

            return sum;
        }

        public string AssessStreak(string symbol, DateTime date)
        {

            string res = "Down";
            double open_st, close_en;

            LoadHistoryBar(symbol, date, 1, "day");
            open_st = intervals[0].GetOpen();
            close_en = intervals[0].GetClose();

            if (open_st <= close_en)
                res = "Up";

            return res;
        }

        public DateTime GetWeekendDate(DateTime inputDate)
        {
            DayOfWeek currentDay = inputDate.DayOfWeek;
            int daysTillCurrentDay = DayOfWeek.Friday - currentDay;
            DateTime currentWeekendDate = inputDate.AddDays(daysTillCurrentDay);
            DateTime output = new DateTime(currentWeekendDate.Year, currentWeekendDate.Month, currentWeekendDate.Day, 16, 30, 0);

            return output;
        }
        public DateTime GetMonthendDate(DateTime inputDate)
        {
            DateTime lastDayOfMonth = new DateTime(inputDate.Year, inputDate.Month, DateTime.DaysInMonth(inputDate.Year, inputDate.Month));
            if (lastDayOfMonth.DayOfWeek != DayOfWeek.Saturday && lastDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                lastDayOfMonth = lastDayOfMonth.AddHours(16.5);
            }

            return lastDayOfMonth;
        }
    }
}
