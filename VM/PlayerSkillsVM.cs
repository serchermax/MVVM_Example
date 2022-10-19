using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsVM 
{
    public event Action<SkillData[]> OnSkillsChanged;
    public event Action<SkillData> OnFirstSkillChanged;
    public event Action<SkillData> OnSecondSkillChanged;

    public static PlayerSkillsVM Instance
    {
        get { return _instance == null ? new PlayerSkillsVM() : _instance; }
    }
    private static PlayerSkillsVM _instance;

    private PlayerSkillsModel _playerSkillsModel;

    private PlayerSkillsVM()
    {
        _instance = this;
        _playerSkillsModel = new PlayerSkillsModel();
    }

    public SkillData[] PlayerSkill => _playerSkillsModel.PlayerSkills;
    public SkillData FirstSkill => _playerSkillsModel.FirstSkill;
    public SkillData SecondSkill => _playerSkillsModel.SecondSkill;

    public void RemoveFirstSkill()
    {
        _playerSkillsModel.RemoveFirstSkill();
        OnFirstSkillChanged?.Invoke(_playerSkillsModel.FirstSkill);
    }

    public void RemoveSecondSkill()
    {
        _playerSkillsModel.RemoveSecondSkill();
        OnSecondSkillChanged?.Invoke(_playerSkillsModel.SecondSkill);
    }

    public void SetFirstSkill(SkillData skill) => SetFirstSkill(_playerSkillsModel.FindSkill(skill));
    public void SetFirstSkill(int index)
    {
        _playerSkillsModel.SetFirstSkill(index);
        OnFirstSkillChanged?.Invoke(_playerSkillsModel.FirstSkill);
    }

    public void SetFirstCustomSkill(SkillData skill)
    {
        _playerSkillsModel.SetFirstCustomSkill(skill);
        OnFirstSkillChanged?.Invoke(_playerSkillsModel.FirstSkill);
    }

    public void SetSecondSkill(SkillData skill) => SetSecondSkill(_playerSkillsModel.FindSkill(skill));
    public void SetSecondSkill(int index)
    {
        _playerSkillsModel.SetSecondSkill(index);
        OnSecondSkillChanged?.Invoke(_playerSkillsModel.SecondSkill);
    }

    public void SetSecondCustomSkill(SkillData skill)
    {
        _playerSkillsModel.SetSecondCustomSkill(skill);
        OnSecondSkillChanged?.Invoke(_playerSkillsModel.SecondSkill);
    }

    public void RemoveSkill(SkillData skill) => RemoveSkill(_playerSkillsModel.FindSkill(skill));
    public void RemoveSkill(int index)
    {
        _playerSkillsModel.RemoveSkill(index);
        OnSkillsChanged?.Invoke(_playerSkillsModel.PlayerSkills);
    }

    public void AddSkill(SkillData skill)
    {
        _playerSkillsModel.AddSkill(skill);
        OnSkillsChanged?.Invoke(_playerSkillsModel.PlayerSkills);
    }

    public void LevelUpSkill(SkillData skill) => LevelUpSkill(_playerSkillsModel.FindSkill(skill));
    public void LevelUpSkill(int index)
    {
        _playerSkillsModel.LevelUpSkill(index);
        OnSkillsChanged?.Invoke(_playerSkillsModel.PlayerSkills);
    }
}
