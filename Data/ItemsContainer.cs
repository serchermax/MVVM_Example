using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer 
{
    private const string MAIN_DIRECTORY = "Items/";
    private readonly string[] DIRECTORIES = 
        { "Swords",
        "Helmets",
        "Armors",
        "Boots",
        "Accessories",
        "Gloves",
        "Sharps",
        "Common", };

    private static ItemsContainer _instance;
    public static ItemsContainer Instance
    {
        get { return _instance == null ? new ItemsContainer() : _instance; }
    }

    private Dictionary<int, Item> _allItems;
    private List<Item> _itemList;
    public int Count => _itemList.Count;

    private ItemsContainer()
    {
        _instance = this;
        _allItems = new Dictionary<int, Item>();
        _itemList = new List<Item>();
        LoadAllFromResources();
    }

    public bool TryGetOriginItemByIndex(int index, out Item item)
    {
        item = null;
        if (_itemList.Count > index) item = _itemList[index];
        return item != null;
    }
    public bool TryGetNewItemByIndex(int index, out Item item)
    {
        item = null;
        if (_itemList.Count > index)
        {
            switch (_itemList[index].ItemCategory)
            {
                case ItemCategories.Equipments: item = new ItemEquipment(_itemList[index] as ItemEquipment); break;
                case ItemCategories.Sharps: item = new ItemSharp(_itemList[index] as ItemSharp); break;
                case ItemCategories.Common: item = new ItemCommon(_itemList[index] as ItemCommon); break;
            }
        }
        return item != null;
    }
    public bool TryGetNewItemById(int id, out Item item)
    {
        item = null;
        if (_allItems.ContainsKey(id))
        {          
            switch (_allItems[id].ItemCategory)
            {
                case ItemCategories.Equipments: item = new ItemEquipment(_allItems[id] as ItemEquipment); break;
                case ItemCategories.Sharps: item = new ItemSharp(_allItems[id] as ItemSharp); break;
                case ItemCategories.Common: item = new ItemCommon(_allItems[id] as ItemCommon); break;
            }
        }
        return item != null;
    }

    public int[] GetID(ItemCategories itemCategory)
    {
        List<int> id = new List<int>();
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].ItemCategory == itemCategory) id.Add(_itemList[i].ID);
        }
        return id.ToArray();
    }
    public int[] GetID(ItemCategories itemCategory, RareTypes rare)
    {
        List<int> id = new List<int>();
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (_itemList[i].ItemCategory == itemCategory && _itemList[i].RareType == rare) id.Add(_itemList[i].ID);
        }
        return id.ToArray();
    }

    private void LoadAllFromResources()
    {
        List<ItemScrObj> temp = new List<ItemScrObj>();

        for (int i = 0; i < DIRECTORIES.Length; i++)
        {
            //Debug.Log(MAIN_DIRECTORY + DIRECTORIES[i]);
            temp.AddRange(Resources.LoadAll<ItemScrObj>(MAIN_DIRECTORY+DIRECTORIES[i]));
            //Debug.Log(temp.Count);
        }
        for (int i = 0; i < temp.Count; i++)
        {
           // Debug.Log(temp[i].Item.Name + temp[i].Item.ID);
            _allItems.Add(temp[i].Item.ID, temp[i].Item);
            _itemList.Add(temp[i].Item);
            if (temp[i].Item is ItemEquipment) (temp[i].Item as ItemEquipment).Refresh();
        }
    }
}
