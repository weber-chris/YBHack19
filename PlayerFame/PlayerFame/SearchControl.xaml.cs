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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlayerFame
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public PlayersViewModel players;

        public SearchControl()
        {
        }

        public delegate void PlayerSelectedHandler(object sender, PlayerModel player);

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var player = players.Players.FirstOrDefault(pl => pl.Name.Equals((string) SearchBox.SelectedItem));
            PlayerSelected?.Invoke(this, player);
        }

        public event PlayerSelectedHandler PlayerSelected;
    }

    public class PlayersViewModel
    {
        public PlayersViewModel(IEnumerable<PlayerModel> players)
        {
            Players = players;
        }

        public IEnumerable<PlayerModel> Players;
        public IEnumerable<string> PlayerNames => Players.Select(player => player.Name);
    }
}
