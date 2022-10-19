using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public static class GameProgressData
    {
        private const string PLAYER_LAST_LEVEL = "player_last_Level";

        public static int LastLevel
        {
            get { return PlayerPrefs.GetInt(PLAYER_LAST_LEVEL, 1); }
            set { PlayerPrefs.SetInt(PLAYER_LAST_LEVEL, value); }
        }
    }
}