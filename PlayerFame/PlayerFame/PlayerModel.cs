using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerFame
{
    public class PlayerModel
    {
        public PlayerModel(string name, string team, string position, string instagramHandle, string instagramFollowers, string twitterHandle, 
                            string twitterFollowers, string fameScore, string transfermarktUri, string sflUri, string pictureUri, double correlation)
        {
            Name = name;
            Team = team;
            Position = position;
            InstagramHandle = instagramHandle;
            InstagramFollowers = instagramFollowers;
            TwitterHandle = twitterHandle;
            TwitterFollowers = twitterFollowers;
            FameScore = fameScore;
            TransfermarktUri = transfermarktUri;
            SflUri = sflUri;
            PictureUri = pictureUri;
            Correlation = correlation;
        }

        public string Name { set; get; }
        public string Team { get; set; }
        public string Position { get; set; }
        public string InstagramHandle { get; set; }
        public string InstagramFollowers { get; set; }
        public string TwitterHandle { get; set; }
        public string TwitterFollowers { get; set; }
        public string FameScore { get; set; }
        public string TransfermarktUri { get; set; }
        public string SflUri { get; set; }
        public string PictureUri { get; set; }
        public double Correlation { get; set; }
    }
}
