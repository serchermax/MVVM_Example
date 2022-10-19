using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlayerSkillsModel 
{
    public virtual SkillData[] PlayerSkills => _playerSkills;
    protected SkillData[] _playerSkills;

    protected SkillData[] _playerSkillsData
    {
        get { return PlayerSkillsData.PlayerSkills; }
        set { PlayerSkillsData.PlayerSkills = value; }
    }

    public virtual SkillData FirstSkill => _firstSkill;
    protected SkillData _firstSkill;

    protected SkillData _firstSkillData
    {
        get { return PlayerSkillsData.FirstSkill; }
        set { PlayerSkillsData.FirstSkill = value; }
    }

    public virtual SkillData SecondSkill => _secondSkill;
    protected SkillData _secondSkill;

    protected SkillData _secondSkillData
    {
        get { return PlayerSkillsData.SecondSkill; }
        set { PlayerSkillsData.SecondSkill = value; }
    }

    public PlayerSkillsModel()
    {
        _playerSkills = _playerSkillsData;
        _firstSkill = _firstSkillData;
        _secondSkill = _secondSkillData;
    }

    public void RemoveFirstSkill()
    {       
        _firstSkill = null;
        _firstSkillData = null;
    }

    public void RemoveSecondSkill()
    {
        _secondSkill = null;
        _secondSkillData = null;
    }

    public void SetFirstSkill(int index)
    {
        _firstSkill = _playerSkills[index];
        _firstSkillData = _firstSkill;
    }
    
    public void SetFirstCustomSkill(SkillData skill)
    {
        _firstSkill = skill;
        _firstSkillData = _firstSkill;
    }

    public void SetSecondSkill(int index)
    {
        _secondSkill = _playerSkills[index];
        _secondSkillData = _secondSkill;
    }

    public void SetSecondCustomSkill(SkillData skill)
    {
        _secondSkill = skill;
        _secondSkillData = _secondSkill;
    }

    public void RemoveSkill(int index)
    {
        _playerSkills[index] = null;
        _playerSkillsData = _playerSkills;
    }

    public void AddSkill(SkillData skill)
    {
        SkillData[] temp = new SkillData[PlayerSkills.Length + 1];

        for (int i = 0; i < PlayerSkills.Length; i++)
            if (temp[i].Skill != skill.Skill) temp[i] = PlayerSkills[i];
            else return;

        temp[temp.Length - 1] = skill;
        _playerSkills = temp;
        _playerSkillsData = _playerSkills;
    }

    public void LevelUpSkill(int index)
    {
        _playerSkills[index] = new SkillData(_playerSkills[index].Skill, _playerSkills[index].Level+1);
        _playerSkillsData = _playerSkills;
    } 

    public int FindSkill(SkillData skill)
    {
        for (int i = 0; i < PlayerSkills.Length; i++)
        {
            if (PlayerSkills[i] == skill)
            {
                return i;
            }
        }
        Debug.LogError("PlayerSkillsModel can't find skill " + skill);
        return 0;
    }

    /*
private void RemoveSkills()
{
  SkillData[] temp = new SkillData[PlayerSkills.Length + 1];

    for (int i = 0; i < PlayerSkills.Length; i++)
        temp[i] = PlayerSkills[i];

    temp[temp.Length - 1] = _secondSkill;
    _secondSkill = null;
    _playerSkills = temp;

    _secondSkillData = null;
    _playerSkillsData = _playerSkills;
}
*/
    // public void SaveTempToData() => _data = Items;
}

[System.Serializable]
public class SkillData
{
    public Skills Skill => _skill;
    [SerializeField] protected Skills _skill;

    public int Level => _level;
    [SerializeField] private int _level;

    public SkillData(Skills skill, int level)
    {
        _skill = skill;
        _level = level;
    }
}