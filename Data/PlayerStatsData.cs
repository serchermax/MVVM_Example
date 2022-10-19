using UnityEngine;

namespace GameData
{
    public static class PlayerStatsData
    {
        private const string PLAYER_BASE_MAXHEALTH = "player_base_MaxHealth";
        private const string PLAYER_BASE_DAMAGE = "player_base_Damage";
        private const string PLAYER_BASE_DODGE = "player_base_Dodge";
        private const string PLAYER_BASE_SPEED = "player_base_Speed";
        private const string PLAYER_BASE_DEFENCE = "player_base_Defence";
        private const string PLAYER_BASE_ACCURACY = "player_base_Accuracy";

        public static float BaseMaxHealth
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_MAXHEALTH, 100); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_MAXHEALTH, value); }
        }

        public static float BaseDamage
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_DAMAGE, 50); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_DAMAGE, value); }
        }

        public static float BaseDodge
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_DODGE, 5); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_DODGE, value); }
        }

        public static float BaseSpeed
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_SPEED, 5); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_SPEED, value); }
        }

        public static float BaseDefence
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_DEFENCE, 0); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_DEFENCE, value); }
        }

        public static float BaseAccuracy
        {
            get { return PlayerPrefs.GetFloat(PLAYER_BASE_ACCURACY, 100); }
            set { PlayerPrefs.SetFloat(PLAYER_BASE_ACCURACY, value); }
        }
    }
}
