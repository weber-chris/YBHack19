using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFame
{
    class DynamicDataProvider
    {
        private IEnumerable<TimeSeriesEntry> timeSeries;
        public IEnumerable<TimeSeriesEntry> TimeSeries
        {
            get
            {
                return timeSeries ??= DynamicPlayerDataParser.ParseDynamicFile(
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\player_dynamic.csv",
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\player_scores_parsed.csv",
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\player_noten.csv",
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\df_insta_timeseries.csv",
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\df_dynamic_performance_merged.csv",
                    "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\df_dynamic_fame_merged.csv"
                    );
            }
        }    
    }
}
