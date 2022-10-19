using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsVM : IDisposable
{
    public event Action<Type, float> OnStatChanged;

    public static PlayerStatsVM Instance
    {
        get { return _instance == null ? new PlayerStatsVM() : _instance; }
    }
    private static PlayerStatsVM _instance;

    private Dictionary<Type, PlayerStatsModel> _playerStats;
    private PlayerStatsEquipBoostAdder _playerStatsEquipBoostAdder;

    private PlayerStatsVM()
    {
        _instance = this;
        _playerStats = new Dictionary<Type, PlayerStatsModel>();

        _playerStats.Add(typeof(PlayerStatDamage), new PlayerStatDamage());
        _playerStats.Add(typeof(PlayerStatMaxHealth), new PlayerStatMaxHealth());
        _playerStats.Add(typeof(PlayerStatDodge), new PlayerStatDodge());
        _playerStats.Add(typeof(PlayerStatSpeed), new PlayerStatSpeed());
        _playerStats.Add(typeof(PlayerStatDefence), new PlayerStatDefence());
        _playerStats.Add(typeof(PlayerStatAccuracy), new PlayerStatAccuracy());

        PlayerStatsModel.OnStatModifed += OnStatModifed;
        _playerStatsEquipBoostAdder = new PlayerStatsEquipBoostAdder(this, EquipmentVM.Instance);
    }

    public float GetStat<T>(bool fromData = false) where T : PlayerStatsModel => fromData
        ? _playerStats[typeof(T)].BaseStat : _playerStats[typeof(T)].Stat;

    public float GetStat(Stats stat, bool fromData = false) => fromData
        ? _playerStats[PlayerStatsTypes.GetTypeFromEnum(stat)].BaseStat
        : _playerStats[PlayerStatsTypes.GetTypeFromEnum(stat)].Stat;

    public void AddStat<T>(float value) where T : PlayerStatsModel
    {
        Type type = typeof(T);
        _playerStats[type].AddStat(value);
        OnStatChanged?.Invoke(type, GetStat<T>());
    }
    public void AddStat(Stats stat, float value) 
    {
        Type type = PlayerStatsTypes.GetTypeFromEnum(stat);
        _playerStats[type].AddStat(value);
        OnStatChanged?.Invoke(type, GetStat(stat));
    }

    public void RemoveStat<T>(float value) where T : PlayerStatsModel
    {
        Type type = typeof(T);
        _playerStats[type].RemoveStat(value);
        OnStatChanged?.Invoke(type, GetStat<T>());
    }
    public void RemoveStat(Stats stat, float value)
    {
        Type type = PlayerStatsTypes.GetTypeFromEnum(stat);
        _playerStats[type].RemoveStat(value);
        OnStatChanged?.Invoke(type, GetStat(stat));
    }

    public IModificator GetIModOf<T>() where T : PlayerStatsModel { return _playerStats[typeof(T)]; }
    public IModificator GetIModOf(Stats stat) { return _playerStats[PlayerStatsTypes.GetTypeFromEnum(stat)]; }

    private void OnStatModifed(Type type, float value) => OnStatChanged?.Invoke(type, value);

    public void Dispose()
    {
        PlayerStatsModel.OnStatModifed -= OnStatModifed;
        _playerStatsEquipBoostAdder.Exit();
    }
}

public enum Stats
{
    Damage = 0,
    MaxHealth = 1,
    Dodge = 2,
    Speed = 3,
    Defence = 4,
    Accuracy = 5
}
public static class PlayerStatsTypes
{
    public static Dictionary<Stats, Type> TypesMap { get; private set; }
    public static Type GetTypeFromEnum(Stats stat)
    {
        if (TypesMap == null) TypesMap = new Dictionary<Stats, Type>();
        if (TypesMap.ContainsKey(stat)) return TypesMap[stat];
        switch (stat)
        {
            case Stats.Damage: return TypesMap[stat] = typeof(PlayerStatDamage);
            case Stats.MaxHealth: return TypesMap[stat] = typeof(PlayerStatMaxHealth);
            case Stats.Dodge: return TypesMap[stat] = typeof(PlayerStatDodge);
            case Stats.Speed: return TypesMap[stat] = typeof(PlayerStatSpeed);
            case Stats.Defence: return TypesMap[stat] = typeof(PlayerStatDefence);
            case Stats.Accuracy: return TypesMap[stat] = typeof(PlayerStatAccuracy);
            default: Debug.LogError("Ошибка получения типа по enum в PlayerStatsModel!"); return null;
        }
    }
}