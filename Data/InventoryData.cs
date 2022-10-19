using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class InventoryData 
    {
        private const string INVENTORY_ITEMS = "inventory_Items";

        public static ItemData[] Item
        {
            get
            {
                string json = PlayerPrefs.GetString(INVENTORY_ITEMS, GetDefaultPack());
                ItemJSONPack itemPack = JsonUtility.FromJson<ItemJSONPack>(json);
                return itemPack.GetItems();
            }
            set
            {
                ItemJSONPack itemPack = new ItemJSONPack();
                itemPack.SetItems(value);
                string json = JsonUtility.ToJson(itemPack);
                PlayerPrefs.SetString(INVENTORY_ITEMS, json);
            }
        }

        private static string GetDefaultPack()
        {
            ItemData[] items = new ItemData[]
            {

            };

            ItemJSONPack itemPack = new ItemJSONPack();
            itemPack.SetItems(items);
            return JsonUtility.ToJson(itemPack);
        }


        [System.Serializable]
        private struct ItemJSONPack
        {
            public ItemSaveData[] ItemSaveData => _itemSaveDatas;
            [SerializeField] private ItemSaveData[] _itemSaveDatas; 

            public void SetItems(ItemData[] data)
            {
                _itemSaveDatas = new ItemSaveData[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Item is ItemEquipment)
                    {
                        ItemEquipment item = data[i].Item as ItemEquipment;
                        _itemSaveDatas[i] = new ItemSaveData(item.ID, data[i].Count, item.RareType, item.Level);
                    }
                    else
                        _itemSaveDatas[i] = new ItemSaveData(data[i].Item.ID, data[i].Count);
                }             
            }            
            public ItemData[] GetItems()
            {
                List<ItemData> temp = new List<ItemData>();

                for (int i = 0; i < _itemSaveDatas.Length; i++)
                {
                    if (_itemSaveDatas[i].TryGetItemData(ItemsContainer.Instance, out ItemData itemData))
                    {
                        temp.Add(itemData);
                    }
                }          
                return temp.ToArray();
            }            
        }                   
    }

    [System.Serializable]
    public class ItemSaveData
    {
        public int ID => _id;
        [SerializeField] protected int _id;

        public int Count => _count;
        [SerializeField] protected int _count;

        public RareTypes Rare => _rare;
        [SerializeField] private RareTypes _rare;

        public int Level => _level;
        [SerializeField] private int _level;

        public ItemSaveData(int id, int count)
        {
            _id = id;
            _count = count;
        }
        public ItemSaveData(int id, int count, RareTypes rare, int level)
        {
            _id = id;
            _count = count;
            _rare = rare;
            _level = level;
        }

        public bool TryGetItemData(ItemsContainer itemsContainer, out ItemData itemData)
        {
            itemData = null;
            if (itemsContainer.TryGetNewItemById(ID, out Item item))
            {
                Item temp = item;
                if (temp is ItemEquipment) (item as ItemEquipment).Refresh(Level, Rare);
                itemData = new ItemData(item, Count);
            }
            return itemData != null;
        }
    }




    /*
    [System.Serializable]
    public class EquipmentSaveData: ItemSaveData 
    {
        public int ID => _id;
        [SerializeField] protected int _id;

        public int Count => _count;
        [SerializeField] protected int _count;

        public RareTypes Rare => _rare;
        [SerializeField] private RareTypes _rare;

        public int Level => _level;
        [SerializeField] private int _level;     

        public EquipmentSaveData(int id, int count, RareTypes rare, int level)
        {
            _id = id;
            _count = count;
            _rare = rare;
            _level = level;
        }     
    }
    */
}