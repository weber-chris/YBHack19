using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace PlayerFame
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl, INotifyPropertyChanged
    {
        private PlayerFameViewModel player;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlayerControl()
        {
            InitializeComponent();

            DataContext = this;

            this.PropertyChanged += PlayerControl_PropertyChanged;

            Chart.AxisY.Add(new Axis
            {
                Foreground = Brushes.BlueViolet,
                MinValue = 0,
                Title = "Market Value",
                LabelFormatter = value => value.ToString("#,#")
            });
            Chart.AxisY.Add(new Axis
            {
                Foreground = Brushes.Red,
                Title = "Performance",
                MinValue = 0,
                MaxValue = 2,
                Position = AxisPosition.RightTop,
                LabelFormatter = value => value.ToString("#.##"),
            });
            Chart.AxisY.Add(new Axis
            {
                Foreground = Brushes.DarkOrange,
                Title = "Fame",
                MinValue = 0,
                MaxValue = 3,
                Position = AxisPosition.RightTop,
                LabelFormatter = value => value.ToString("#.##"),
                
            });

            Chart.AxisY[0].Separator.StrokeThickness = 0;
            Chart.AxisY[1].Separator.StrokeThickness = 0;
            Chart.AxisY[2].Separator.StrokeThickness = 0;
            Chart.AxisY[3].Separator.StrokeThickness = 0;
        }

        private void PlayerControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Chart.Series.Clear();

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double)dayModel.DateTime.Ticks)
                .Y(dayModel => dayModel.Value);

            var sortedByDate = Player.TimeSeries.OrderBy(entry => entry.Date).ToArray();

            var firstNonPerformanceDate = sortedByDate.FirstOrDefault(entry => entry.Type != TimeSeriesType.PerformanceCombined && entry.Type != TimeSeriesType.Performance)?.Date;
            firstNonPerformanceDate ??= new DateTime(2019, 1, 1);

            var marketValues = sortedByDate.Where(entry => entry.Type == TimeSeriesType.MarketValue).Select(entry => new DateModel(entry.Date, entry.Value));
            var marketChartValues = new ChartValues<DateModel>(marketValues);

            var performanceValues = sortedByDate.Where(entry => entry.Date.CompareTo(firstNonPerformanceDate) > 0 && entry.Type == TimeSeriesType.PerformanceCombined).Select(entry => new DateModel(entry.Date, entry.Value));
            var performanceChartValues = new ChartValues<DateModel>(performanceValues);

            var fameValues = sortedByDate.Where(entry => entry.Type == TimeSeriesType.Fame).Select(entry => new DateModel(entry.Date, entry.Value));
            var fameChartValues = new ChartValues<DateModel>(fameValues);

            Chart.Series = new SeriesCollection(dayConfig)
            {
                new LineSeries { ScalesYAt = 1, Values = marketChartValues, Title = "Marktwert", Stroke = Brushes.BlueViolet },
                new LineSeries { ScalesYAt = 2, LineSmoothness = 1, PointGeometry = null, Values = performanceChartValues, Title = "Performance", Stroke = Brushes.Red },
                new LineSeries { ScalesYAt = 3, LineSmoothness = 1, PointGeometry = null, Values = fameChartValues, Title = "Fame", Stroke = Brushes.DarkOrange },
            };

            Formatter = value => new System.DateTime((long)value).ToString("dd.MM.yyyy");
            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }

        public PlayerFameViewModel Player
        {
            get => player;
            set
            {
                player = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DateModel
    {
        public DateModel(DateTime dateTime, double value)
        {
            DateTime = dateTime;
            Value = value;
        }

        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
