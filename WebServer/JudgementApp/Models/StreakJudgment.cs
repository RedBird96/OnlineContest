﻿using System;
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

    public abstract class Holiday
    {
        public abstract DateTime? GetDate(int year);

        public static DateTime GetNextNonHolidayWeekDay(DateTime date, IList<Holiday> holidays, IList<DayOfWeek> weekendDays)
        {
            // always start with tomorrow, and truncate time to be safe
            date = date.Date.AddDays(1);

            // calculate holidays for both this year and next year
            var holidayDates = holidays.Select(x => x.GetDate(date.Year))
                .Union(holidays.Select(x => x.GetDate(date.Year + 1)))
                .Where(x => x != null)
                .Select(x => x.Value)
                .OrderBy(x => x).ToArray();

            // increment until we get a non-weekend and non-holiday date
            while (true)
            {
                if (weekendDays.Contains(date.DayOfWeek) || holidayDates.Contains(date))
                    date = date.AddDays(1);
                else
                    return date;
            }
        }
    }

    public class MonthDayBasedHoliday : Holiday
    {
        private readonly int _month;
        private readonly int _day;

        public MonthDayBasedHoliday(int month, int day)
        {
            _month = month;
            _day = day;
        }

        public override DateTime? GetDate(int year)
        {
            return new DateTime(year, _month, _day);
        }
    }

    public class DayOfWeekBasedHoliday : Holiday
    {
        private readonly int _occurrence;
        private readonly DayOfWeek _dayOfWeek;
        private readonly int _month;

        public DayOfWeekBasedHoliday(int occurrence, DayOfWeek dayOfWeek, int month)
        {
            _occurrence = occurrence;
            _dayOfWeek = dayOfWeek;
            _month = month;
        }

        public override DateTime? GetDate(int year)
        {
            if (_occurrence <= 4)
            {
                DateTime dt = new DateTime(year, _month, 1);
                int delta = (_dayOfWeek - dt.DayOfWeek + 7) % 7;
                delta += 7 * (_occurrence - 1);
                return dt.AddDays(delta);
            }
            else  // last occurrence in month
            {
                int daysInMonth = DateTime.DaysInMonth(year, _month);
                DateTime dt = new DateTime(year, _month, daysInMonth);
                int delta = (dt.DayOfWeek - _dayOfWeek + 7) % 7;
                return dt.AddDays(-delta);
            }
        }
    }

    public class FixedDateBasedHoliday : Holiday
    {
        private readonly IDictionary<int, DateTime> _dates;

        public FixedDateBasedHoliday(params DateTime[] dates)
        {
            _dates = dates.ToDictionary(x => x.Year, x => x);
        }

        public override DateTime? GetDate(int year)
        {
            if (_dates.ContainsKey(year))
                return _dates[year];

            // fixed date not established for year
            return null;
        }
    }
}