using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressVM 
{
    public event Action<Type, int> OnLevelProgressChanged;

    public static GameProgressVM Instance
    {
        get { return _instance == null ? _instance = new GameProgressVM() : _instance; }
    }
    private static GameProgressVM _instance;

    private Dictionary<Type, GameProgressModel> _gameProgress;

    private GameProgressVM()
    {
        _gameProgress = new Dictionary<Type, GameProgressModel>();

        _gameProgress.Add(typeof(GameProgressLastLevel), new GameProgressLastLevel());
    }

    public int GetValue<T>() where T : GameProgressModel => _gameProgress[typeof(T)].Value;

    public void AddValue<T>(int value) where T : GameProgressModel
    {
        Type type = typeof(T);
        _gameProgress[type].AddValue(value);
        OnLevelProgressChanged?.Invoke(type, GetValue<T>());
    }

    public void RemoveValue<T>(int value) where T : GameProgressModel
    {
        Type type = typeof(T);
        _gameProgress[type].RemoveValue(value);
        OnLevelProgressChanged?.Invoke(type, GetValue<T>());
    }
}
