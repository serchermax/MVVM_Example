using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class PlayerSkillsData
    {
        private const string PLAYER_SKILLS = "player_skills";
        private const string PLAYER_FIRST_SKILL = "player_first_skill";
        private const string PLAYER_SECOND_SKILL = "player_second_skill";

        public static SkillData[] PlayerSkills
        {
            get
            {
                string json = PlayerPrefs.GetString(PLAYER_SKILLS, GetDefaultPack());
                SkillsJSONPack itemPack = JsonUtility.FromJson<SkillsJSONPack>(json);
                return itemPack.SkillData;
            }
            set
            {
                SkillsJSONPack itemPack = new SkillsJSONPack();
                itemPack.SetSkills(value);
                string json = JsonUtility.ToJson(itemPack);
                PlayerPrefs.SetString(PLAYER_SKILLS, json);
            }
        }

        public static SkillData FirstSkill
        {
            get
            {
                return new SkillData(Skills.Regeneration, 1);
                string json = PlayerPrefs.GetString(PLAYER_FIRST_SKILL, "");
                return JsonUtility.FromJson<SkillData>(json);
            }
            set
            {
                string json = JsonUtility.ToJson(value);
                PlayerPrefs.SetString(PLAYER_FIRST_SKILL, json);
            }
        }

        public static SkillData SecondSkill
        {
            get
            {
                return new SkillData(Skills.BigGuy, 1);
                string json = PlayerPrefs.GetString(PLAYER_SECOND_SKILL, "");
                return JsonUtility.FromJson<SkillData>(json);
            }
            set
            {
                string json = JsonUtility.ToJson(value);
                PlayerPrefs.SetString(PLAYER_SECOND_SKILL, json);
            }
        }

        private static string GetDefaultPack()
        {
            SkillData[] skills = new SkillData[]
            {
                new SkillData(Skills.SurgeOfRage, 1),
                new SkillData(Skills.Vampiric, 1)
            };      

            SkillsJSONPack skillsPack = new SkillsJSONPack();
            skillsPack.SetSkills(skills);
            return JsonUtility.ToJson(skills);
        }


        [System.Serializable]
        private struct SkillsJSONPack
        {
            public SkillData[] SkillData => _skillData;
            [SerializeField] private SkillData[] _skillData;

            public void SetSkills(SkillData[] data)
            {
                _skillData = data;
            }           
        }
    }
}