using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public abstract class PlayerStatsModel : IModificator
{
    public static event Action<Type, float> OnStatModifed;
    public virtual float Stat 
    {
        get { return _stat + _generalModificator > _minStatValue ? _stat + _generalModificator : _minStatValue; }
    }
    public virtual float BaseStat => _stat;

    protected abstract float data { get; set; }
    protected float _stat;   
    protected float _minStatValue = 0;

    protected Dictionary<object, float> modificators = new Dictionary<object, float>();
    protected float _generalModificator;

    public PlayerStatsModel()
    {
        _stat = data;
    }

    public void AddStat(float value) 
    {
        data += value;
        _stat = data;
    }
    public virtual void RemoveStat(float value) 
    {
        data = _stat - value > _minStatValue ? _stat - value : _minStatValue;
        _stat = data;
    }

    public void AddModificator(object obj, float newModificator, bool muliply = true)
    {
        if (modificators.ContainsKey(obj)) return;
             
        float mod = muliply ? _stat * newModificator : newModificator;
        _generalModificator += mod;
        modificators.Add(obj, mod);

        OnStatModifed?.Invoke(GetType(), Stat);
    }

    public void RemoveModificator(object obj)
    {
        if (modificators.ContainsKey(obj))
        {
            _generalModificator -= modificators[obj];
            modificators.Remove(obj);
        }
        OnStatModifed?.Invoke(GetType(), Stat);
    }
}

public class PlayerStatDamage : PlayerStatsModel
{
    public PlayerStatDamage() { _minStatValue = 1; }
    protected override float data
    {
        get { return PlayerStatsData.BaseDamage; }
        set { PlayerStatsData.BaseDamage = value; }
    }
}

public class PlayerStatMaxHealth : PlayerStatsModel
{
    public PlayerStatMaxHealth() { _minStatValue = 1; }
    protected override float data
    {
        get { return PlayerStatsData.BaseMaxHealth; }
        set { PlayerStatsData.BaseMaxHealth = value; }
    }
}

public class PlayerStatDodge : PlayerStatsModel
{
    public PlayerStatDodge() { _minStatValue = 0; }
    protected override float data
    {
        get { return PlayerStatsData.BaseDodge; }
        set { PlayerStatsData.BaseDodge = value; }
    }
}

public class PlayerStatSpeed : PlayerStatsModel
{
    public PlayerStatSpeed() { _minStatValue = 0.5f; }
    protected override float data
    {
        get { return PlayerStatsData.BaseSpeed; }
        set { PlayerStatsData.BaseSpeed = value; }
    }
}

public class PlayerStatDefence: PlayerStatsModel
{
    public PlayerStatDefence() { _minStatValue = -100f; }
    protected override float data
    {
        get { return PlayerStatsData.BaseDefence; }
        set { PlayerStatsData.BaseDefence = value; }
    }
}

public class PlayerStatAccuracy : PlayerStatsModel
{
    public PlayerStatAccuracy() { _minStatValue = 5f; }
    protected override float data
    {
        get { return PlayerStatsData.BaseAccuracy; }
        set { PlayerStatsData.BaseAccuracy = value; }
    }
}