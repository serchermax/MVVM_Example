using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class InventoryModel 
{
    public virtual ItemData[] Items => _items;
    protected ItemData[] _items;
    protected ItemData[] _data
    {
        get { return InventoryData.Item;  }
        set { InventoryData.Item = value; }
    }

    public InventoryModel()
    {
        _items = _data;
    }

    public bool TryGetItemDataOfType<T>(int index, out ItemData itemData) where T : Item
    {
        itemData = Items[index];
        return Items[index].Item is T;
    }

    public bool TryGetItemDatasOfType<T>(out ItemData[] item) where T : Item
    {
        List<ItemData> temp = new List<ItemData>();
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].Item is T) temp.Add(_items[i]); 
        }
        item = temp.ToArray();
        return item.Length > 0;
    }

    public void AddItem(Item item, int count)
    {        
        count = count < 1 ? 1 : count;
        if (item.IsStackable && TryFindItemDataByID(item.ID, out int index))
        {
            ItemData replace = new ItemData(_items[index].Item, _items[index].Count + count);
            _items[index] = replace;
        }
        else
        {
            List<ItemData> data = new List<ItemData>();
            if (item.IsStackable)
            {
                data.Add(new ItemData(item, count));
            }
            else
            {                
                for (int i = 0; i < count; i++)
                    data.Add(new ItemData(item, 1));
            }

            List<ItemData> newDatas = _items == null ? new List<ItemData>() : new List<ItemData>(_items);
            newDatas.AddRange(data);
            _items = newDatas.ToArray();
        }
        _data = _items;
    }

    private bool TryFindItemDataByID(int ID, out int index)
    {
        index = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].Item.ID == ID)
            {
                index = i;
                return true;
            }
        }
        return false;       
    }

    public virtual void RemoveItem(int index, int count)
    {
        if (_items == null) return;
        if (_items.Length <= index) return;               
        count = count < 1 ? 1 : count;

        if (_items[index].Item.IsStackable)
        {
            ItemData temp = new ItemData(_items[index].Item, _items[index].Count - count);
            _items[index] = temp;
            if (_items[index].Count < 1)
                DeleteItem(index);            
        }
        else DeleteItem(index);

        _data = _items;
    }

    private void DeleteItem(int index)
    {
        ItemData[] temp = new ItemData[_items.Length - 1];
        for (int i = 0; i < _items.Length; i++)
        {
            if (i == index) continue;
            temp[i >= index ? i - 1 : i] = _items[i];
        }
        _items = temp;
    }

    public bool TryFindItem(ItemData itemData, out int index)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == itemData)
            {
                index = i;
                return true;
            }
        }
        index = 0;
        return false;
    }

    public void SaveTempToData() => _data = Items;

    /*
    public virtual void RemoveItem(Item item)
    {
        List<Item> newItems = new List<Item>(_items);
        newItems.Remove(item);
        _items = newItems.ToArray();
        data = _items;
    }
    */
}

[System.Serializable]
public class ItemData
{
    public Item Item => _item;
    [SerializeField] private Item _item;

    public int Count => _count;
    [SerializeField] private int _count;

    public ItemData(Item item, int count)
    {
        _item = item;
        _count = count;
    }
}