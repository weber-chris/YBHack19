using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlayerFame
{
    class StaticDataParser
    {
        public static IEnumerable<PlayerModel> ParsePlayerData(string path, string correlationPath)
        {
            var corellationData = ParseCorrelationData(correlationPath);
            return ParseStaticData(path, corellationData);
        }

        private static IEnumerable<PlayerModel> ParseStaticData(string path, Dictionary<string, double> correlationData)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var file = new StreamReader(fileStream);
            var rows = file.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var headers = rows[0].Split(",");
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < headers.Length; i++)
            {
                dictionary[headers[i]] = i;
            }

            return rows.Skip(1).Select(row =>
            {
                var split = row.Split(",");
                var name = split[dictionary["name"]];
                return new PlayerModel(name, split[dictionary["team"]], MapPosition(split[dictionary["position"]]), split[dictionary["name_instagram"]],
                    FormatIntValue(split[dictionary["follower_instagram"]]), split[dictionary["name_twitter"]], FormatIntValue(split[dictionary["follower_twitter"]]),
                    split[dictionary["fame"]], $"http://transfermarkt.ch{split[dictionary["url_trans"]]}", "", split[dictionary["url_picture"]], correlationData.GetValueOrDefault(name, -1.0));
            });
        }
        private static Dictionary<string, double> ParseCorrelationData(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var file = new StreamReader(fileStream);
            var rows = file.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var headers = rows[0].Split(",");
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < headers.Length; i++)
            {
                dictionary[headers[i]] = i;
            }

            var dict = new Dictionary<string, double>();
            foreach(var row in rows.Skip(1))
            {
                var split = row.Split(",");
                dict[split[dictionary["name"]]] = double.Parse(split[dictionary["correlation"]]);
            }

            return dict;
        }

        private static string MapPosition(string pos)
        {
            return pos switch
            {
                "Goalkeeper" => "TORHÜTER",
                "Centre-Back" => "INNENVERTEIDIGER",
                "Left-Back" => "LINKER VERTEIDIGER",
                "Right-Back" => "RECHTER VERTEIDIGER",
                "DefensiveMidfield" => "DEFENSIVES MITTELFELD",
                "CentralMidfield" => "ZENTRALES MITTELFELD",
                "RightMidfield" => "RECHTES MITTELFELD",
                "LeftMidfield" => "LINKES MITTELFELD",
                "LeftWinger" => "LINKER FLÜGEL",
                "RightWinger" => "RECHTER FLÜGEL",
                "Centre-Forward" => "MITTELSTÜRMER",
                _ => ""
            };
        }

        private static string FormatIntValue(string value, string prefix = "")
        {
            var parsed = int.Parse(string.IsNullOrEmpty(value) || value.Contains(".") ? "0" : value).ToString($"{prefix}#,#");
            return string.IsNullOrEmpty(parsed) ? "0" : parsed;
        }
    }
}
