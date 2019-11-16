using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;

namespace PlayerFame
{
    public class PlayerFameViewModel : INotifyPropertyChanged
    {
        private PlayerModel player;
        private IList<TimeSeriesEntry> _timeSeries = new List<TimeSeriesEntry>();

        public event PropertyChangedEventHandler PropertyChanged;

        public PlayerFameViewModel(PlayerModel player, IEnumerable<TimeSeriesEntry> timeSeries)
        {
            Player = player;
            var asArray = timeSeries.ToArray();
            for (var i = 0; i < asArray.Length; i++)
            {
                if (i == 0 || asArray[i].Type != TimeSeriesType.Performance)
                {
                    _timeSeries.Add(asArray[i]);
                } else
                {
                    var inRange = asArray[..(i + 1)].Where(other =>
                        other.Type == asArray[i].Type && (asArray[i].Date - other.Date).TotalDays < 28.0).Select(e => e.Value);
                    var score = inRange.Any() ? (int)inRange.Average() : asArray[i].Value;
                    _timeSeries.Add(new TimeSeriesEntry(asArray[i].Name, asArray[i].Date, score, asArray[i].Type));
                }
            }

            var lastMarketValue = _timeSeries.LastOrDefault(entry => entry.Type == TimeSeriesType.MarketValue);
            if (lastMarketValue != null)
            {
                _timeSeries.Add(new TimeSeriesEntry(lastMarketValue.Name, DateTime.Today, lastMarketValue.Value,
                    lastMarketValue.Type));
            }
        }
        public PlayerModel Player
        {
            get => player;
            set
            {
                player = value;
                NotifyPropertyChanged();
            }
        }
        public IList<TimeSeriesEntry> TimeSeries
        {
            get => _timeSeries;
            set
            {
                _timeSeries = value;
                NotifyPropertyChanged();
            }
        }

        public string CurrentMarketValue => _timeSeries.Where(entry => entry.Type == TimeSeriesType.MarketValue)
                                                .OrderBy(entry => entry.Date).LastOrDefault()?.Value.ToString("CHF #,#") ?? "";


        public string CurrentPressReputation => _timeSeries.Where(entry => entry.Type == TimeSeriesType.Grade)
                                             .OrderBy(entry => entry.Date).LastOrDefault()?.Value.ToString("") ?? "";

        public float CurrentPerformanceScore => (float)(_timeSeries.Where(entry => entry.Type == TimeSeriesType.Performance)
                                                            .OrderBy(entry => entry.Date).LastOrDefault()?.Value ?? 0.0f);

        public float CurrentCombinedPerformanceScore => (float)(_timeSeries.Where(entry => entry.Type == TimeSeriesType.PerformanceCombined)
                                                                    .OrderBy(entry => entry.Date).LastOrDefault()?.Value ?? 0.0f);
        
        public float FameScore => (float)(_timeSeries.Where(entry => entry.Type == TimeSeriesType.Fame)
                                                                    .OrderBy(entry => entry.Date).LastOrDefault()?.Value ?? 0.0f);

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
