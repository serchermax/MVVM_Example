using UnityEngine;

namespace GameData
{
    public class PlayerGoldData
    {
        private const string PLAYER_GOLD = "player_Gold";

        public int Gold
        {
            get { return PlayerPrefs.GetInt(PLAYER_GOLD, 10000); }
            set { PlayerPrefs.SetInt(PLAYER_GOLD, value); }
        }
    }
}
