using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public abstract class GameProgressModel 
{
    public virtual int Value => _value;
    protected int _value;

    protected abstract int data { get; set; }
    protected int _minStatValue = 0;

    public GameProgressModel()
    {
        _value = data;
    }

    public void AddValue(int value)
    {
        data += value;
        _value = data;
    }
    public virtual void RemoveValue(int value)
    {
        data = _value - value > _minStatValue ? _value - value : _minStatValue;
        _value = data;
    }
}

public class GameProgressLastLevel : GameProgressModel
{
    public GameProgressLastLevel() { _minStatValue = 1; }
    protected override int data
    {
        get { return GameProgressData.LastLevel; }
        set { GameProgressData.LastLevel = value; }
    }
}