using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryVM : IDisposable
{
    public delegate void ItemListenerDelegate(ItemData item);
    private Dictionary<int, ItemListenerDelegate> _onItemChanged;
    public event Action<ItemData[]> OnItemsChanged;

    public static InventoryVM Instance
    {
        get { return _instance == null ? new InventoryVM() : _instance; }
    }
    private static InventoryVM _instance;

    private InventoryModel _inventoryModel;
    private ItemsContainer _itemsContainer;

    private InventoryVM()
    {
        _instance = this;
        _inventoryModel = new InventoryModel();
        _itemsContainer = ItemsContainer.Instance;
        _onItemChanged = new Dictionary<int, ItemListenerDelegate>();
        Item.OnItemChanged += ItemChanged;
    }

    public ItemData[] ItemDatas => _inventoryModel.Items;
    public Item GetItem(int index) => _inventoryModel.Items[index].Item;  
    public int GetCount(int index) => _inventoryModel.Items[index].Count;  
    
    public bool TryGetItemDataOfType<T>(ItemData itemDataToCheck) where T : Item
    {
        if (_inventoryModel.TryFindItem(itemDataToCheck, out int index)) return TryGetItemDataOfType<T>(index, out itemDataToCheck);
        else return false;
    }
    public bool TryGetItemDataOfType<T>(int index, out ItemData itemData) where T : Item
    {
        return _inventoryModel.TryGetItemDataOfType<T>(index, out itemData);
    }
    public bool TryGetItemsOfType<T>(out ItemData[] itemData) where T : Item => _inventoryModel.TryGetItemDatasOfType<T>(out itemData);

    public void AddItem(int id, int count = 1)
    {
        if (_itemsContainer.TryGetNewItemById(id, out Item item))
            AddItem(item, count);
        else Debug.LogError("InventoryVM can't take item with id " + id + " from ItemsContainer!");
    }
    public void AddItem(ItemData itemData) => AddItem(itemData.Item, itemData.Count);
    public void AddItem(Item item, int count = 1)
    {
        _inventoryModel.AddItem(item, count);
        OnItemsChanged?.Invoke(_inventoryModel.Items);
    }

    public virtual void RemoveItem(ItemData itemData, int count = 1) 
    {
        if (_inventoryModel.TryFindItem(itemData, out int index)) RemoveItem(index, count);
        else Debug.LogError("InventoryVM can't remove item from itemData " + itemData);
    }
    public virtual void RemoveItem(int index, int count = 1)
    {
        _inventoryModel.RemoveItem(index, count);
        if (_onItemChanged.ContainsKey(index)) _onItemChanged[index]?.Invoke(null);
        OnItemsChanged?.Invoke(_inventoryModel.Items);
    }

    public bool FindItem<T>(int ID, int count = 1, bool tryUseIt = false) where T : Item
    {
        if (TryGetItemsOfType<T>(out ItemData[] itemData))
        {
            if (FindItem(ID, itemData, out ItemData found))
            {
                if (found.Count >= count)
                {
                    if (tryUseIt) RemoveItem(found, count);
                    return true;
                }
            }
        }
        return false;
    }
    public bool FindItem(int ID, int count = 1, bool tryUseIt = false)
    {
        if (FindItem(ID, ItemDatas, out ItemData found))
        {
            if (found.Count >= count)
            {
                if (tryUseIt) RemoveItem(found, count);
                return true;
            }
        }
        return false;
    }
    private bool FindItem(int ID, ItemData[] itemData, out ItemData found)
    {
        found = null;
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i].Item.ID == ID)
            {
                found = itemData[i];
                return true;
            }
        }
        return false;
    }

    public void AddListener(ItemListenerDelegate itemListenerDelegate, int index)
    {
        if (_onItemChanged.ContainsKey(index))
        {
            _onItemChanged[index] += itemListenerDelegate;   
        }
        else _onItemChanged.Add(index, itemListenerDelegate);
    }
    public void RemoveListener(ItemListenerDelegate itemListenerDelegate, int index)
    {
        if (_onItemChanged.ContainsKey(index))
        {
            _onItemChanged[index] -= itemListenerDelegate;
        }
    }

    public void Dispose()
    {
        Item.OnItemChanged -= ItemChanged;
    }
    
    private void ItemChanged(Item item)
    {
        _inventoryModel.SaveTempToData();
        OnItemsChanged?.Invoke(ItemDatas);
    }
}
