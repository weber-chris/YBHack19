using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlayerFame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlayersViewModel Players;
        private DynamicDataProvider dynamicDataProvider;
        private StaticDataProvider staticDataProvider;

        public MainWindow()
        {
            InitializeComponent();

            dynamicDataProvider = new DynamicDataProvider();

        //    Players = new PlayersViewModel(new[] { new PlayerModel
        //        {
        //            FameScore = "486", InstagramFollowers = "4'592", MarketValue = "CHF 750'000", Name = "Cédric Zesiger",
        //            Position = "Verteidiger", Team = "BSC Young Boys", TwitterFollowers = "149",
        //            SflUri =
        //                "https://www.sfl.ch/superleague/klubs/bsc-young-boys/player/cedric-adrian-zesiger/season/201920/",
        //            PictureUri = "https://www.bscyb.ch/cgi-bin/spieler/301-spieler-20190710112923.jpg"
        //        },
        //        new PlayerModel
        //        {
        //            FameScore = "-200", InstagramFollowers = "1", MarketValue = "CHF 3.50", Name = "Christoph Weber",
        //            Position = "Lutscher", Team = "Globus Boys", TwitterFollowers = "420",
        //            SflUri =
        //                "https://www.sfl.ch/superleague/klubs/bsc-young-boys/player/cedric-adrian-zesiger/season/201920/",
        //            PictureUri = "https://media.licdn.com/dms/image/C4D03AQF7ArpNO1PT4A/profile-displayphoto-shrink_200_200/0?e=1579132800&v=beta&t=bpBpff-iZWdrejysA0Jz22PLR-H8vWlTdOMobdS2rf4"
        //        },
        //        new PlayerModel
        //        {
        //            FameScore = ">9000",
        //            InstagramFollowers = "666",
        //            MarketValue = "CHF 1'000'000",
        //            Name = "David '(H)Acki' Ackermann",
        //            Position = "Uncertified Hacker",
        //            Team = "VBA Warriors",
        //            TwitterFollowers = "163",
        //            SflUri =
        //                "https://www.sfl.ch/superleague/klubs/bsc-young-boys/player/cedric-adrian-zesiger/season/201920/",
        //            PictureUri = "https://pbs.twimg.com/profile_images/441151936029409280/IpipSwbo_400x400.jpeg"
        //        }
        //} 
        //    );
            staticDataProvider = new StaticDataProvider();
            Players = new PlayersViewModel(staticDataProvider.Players);
            DataContext = Players;
        }

        private void SearchBox_OnSelectionChanged(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key != Key.Enter)
            {
                return;
            }

            var player = Players.Players.FirstOrDefault(pl => pl.Name.Equals((string) SearchBox.SelectedItem));
            if (player != null)
            {
                SearchView.Visibility = Visibility.Collapsed;
                DataContext = player;
                Player.Player = BuildFameViewModel(player);
                PlayerView.Visibility = Visibility.Visible;
            }
        }

        private PlayerFameViewModel BuildFameViewModel(PlayerModel player)
        {
            var timeSeries = dynamicDataProvider.TimeSeries;

            return new PlayerFameViewModel(player, timeSeries.Where(entry => entry.Name == player.Name));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SearchView.IsVisible)
            {
                SearchView.Visibility = Visibility.Visible;
                PlayerView.Visibility = Visibility.Collapsed;
                DataContext = Players;
            }
        }
    }
}
