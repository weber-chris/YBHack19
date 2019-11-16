using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFame
{
    public class StaticDataProvider
    {
        private IEnumerable<PlayerModel> players;

        public IEnumerable<PlayerModel> Players => players ??= StaticDataParser.ParsePlayerData("C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\df_static_merged.csv", "C:\\Users\\NicoStrebel\\source\\repos\\YBHack19\\data\\df_correlations.csv");
    }
}
