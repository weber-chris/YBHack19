using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFame
{
    public class TimeSeriesEntry
    {
        public TimeSeriesEntry(string name, DateTime date, double value, TimeSeriesType type)
        {
            Name = name;
            Date = date;
            Value = value;
            Type = type;
        }

        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public TimeSeriesType Type { get; set; }
    }

    public enum TimeSeriesType
    {
        MarketValue,
        Performance,
        Grade,
        InstaLikes,
        PerformanceCombined,
        Fame
    }
}