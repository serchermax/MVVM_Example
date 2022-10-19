using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldVM
{
    public event Action<int> OnSavedGoldChanged;
    public event Action<int> OnGoldChanged;

    public static PlayerGoldVM Instance
    {
        get { return _instance == null ? new PlayerGoldVM() : _instance; }
    }
    private static PlayerGoldVM _instance;

    private PlayerGoldModel _playerGoldModel;

    private PlayerGoldVM()
    {
        _instance = this;
        _playerGoldModel = new PlayerGoldModel();        
    }

    public void SaveGold() => _playerGoldModel.SaveGold();
    public int GetGold() => _playerGoldModel.Gold;
    public void AddGold(int value) => OnGoldChanged?.Invoke(_playerGoldModel.AddGold(value));
    public bool TrySpendGold(int value)
    {
        if (_playerGoldModel.TrySpendGold(value))
        {
            OnGoldChanged(_playerGoldModel.Gold);
            return true;
        }
        else return false;
    }

    public int GetSavedGold() => _playerGoldModel.GetSavedGold();    
    public void AddToSavedGold(int value) => OnSavedGoldChanged?.Invoke(_playerGoldModel.AddToSavedGold(value));    
    public bool TrySpendFromSavedGold(int value)
    {
        if (_playerGoldModel.TrySpendFromSavedGold(value))
        {
            OnSavedGoldChanged(_playerGoldModel.GetSavedGold());
            return true;
        }
        else return false;
    }
}
