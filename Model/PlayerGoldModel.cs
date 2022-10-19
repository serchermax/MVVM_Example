using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlayerGoldModel 
{
    public int Gold { get; private set; }

    private PlayerGoldData _playerGoldData;  

    public PlayerGoldModel()
    {
        _playerGoldData = new PlayerGoldData();
    }

    public int AddGold(int value)
    {
        Gold += value;
        return Gold;
    }      
    public bool TrySpendGold(int value)
    {
        if (Gold >= value)
        {
            Gold -= value;
            return true;
        }
        else return false;
    }

    public void SaveGold() { _playerGoldData.Gold += Gold; Gold = 0; }

    public int GetSavedGold() => _playerGoldData.Gold;
    public int AddToSavedGold(int value)
    {
        _playerGoldData.Gold += value;
        return _playerGoldData.Gold;       
    }
    public bool TrySpendFromSavedGold(int value)
    {
        if (_playerGoldData.Gold >= value)
        {
            _playerGoldData.Gold -= value;
            return true;
        }
        else return false;
    }
}
