using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PlayerFame
{
    static class DynamicPlayerDataParser
    {
        public static IEnumerable<TimeSeriesEntry> ParseDynamicFile(string marketValuePath, string performancePath, string gradePath, string instaLikePath, string performanceCombinedPath, string famePath)
        {
            var marketValueFileStream = new FileStream(marketValuePath, FileMode.Open, FileAccess.Read);
            var marketValueFile = new StreamReader(marketValueFileStream);
            var marketRows = marketValueFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var marketValues = marketRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                return new TimeSeriesEntry(split[0], DateTime.Parse(split[1]), int.Parse(split[2]), TimeSeriesType.MarketValue);
            });

            var performanceFileStream = new FileStream(performancePath, FileMode.Open, FileAccess.Read);
            var performanceFile = new StreamReader(performanceFileStream);
            var performanceRows = performanceFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var performanceValues = performanceRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                try
                {
                    return new TimeSeriesEntry(split[0],
                        DateTime.ParseExact(split[2], "dd.MM.yyyy", CultureInfo.InvariantCulture), int.Parse(split[1]),
                        TimeSeriesType.Performance);
                }
                catch (Exception)
                {
                    return null;
                }
            }).Where(r => r != null);

            var gradeFileStream = new FileStream(gradePath, FileMode.Open, FileAccess.Read);
            var gradeFile = new StreamReader(gradeFileStream);
            var gradeRows = gradeFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var gradeValues = gradeRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                var value = double.Parse(split[1].Trim());
                return new TimeSeriesEntry(split[0], DateTime.ParseExact(split[2], "yyyy-MM-dd", CultureInfo.InvariantCulture), value, TimeSeriesType.Grade);
            });

            var instaFileStream = new FileStream(instaLikePath, FileMode.Open, FileAccess.Read);
            var instaFileFile = new StreamReader(instaFileStream);
            var instaFileRows = instaFileFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var instaFileValues = instaFileRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                var value = double.Parse(split[2].Trim());
                return new TimeSeriesEntry(split[0], DateTime.ParseExact(split[1][..10], "yyyy-MM-dd", CultureInfo.InvariantCulture), value, TimeSeriesType.InstaLikes);
            });

            var performanceCombinedFileStream = new FileStream(performanceCombinedPath, FileMode.Open, FileAccess.Read);
            var performanceCombinedFile = new StreamReader(performanceCombinedFileStream);
            var performanceCombinedFileRows = performanceCombinedFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var performanceCombinedFileValues = performanceCombinedFileRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                return new TimeSeriesEntry(split[0], DateTime.Parse(split[1]), double.Parse(split[6]), TimeSeriesType.PerformanceCombined);
            });

            var fameStream = new FileStream(famePath, FileMode.Open, FileAccess.Read);
            var fameFile = new StreamReader(fameStream);
            var fameRows = fameFile.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var fameValues = fameRows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                return new TimeSeriesEntry(split[0], DateTime.Parse(split[1]), double.Parse(split[6]), TimeSeriesType.Fame);
            });

            return marketValues.Concat(performanceValues).Concat(gradeValues).Concat(instaFileValues).Concat(performanceCombinedFileValues).Concat(fameValues);
        }
    }
}