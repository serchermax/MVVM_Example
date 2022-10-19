using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class EquipmentData 
    {
        private const string EQUIPMENT_SWORD = "equipment_Sword";
        private const string EQUIPMENT_HELMET = "equipment_Helmet";
        private const string EQUIPMENT_ARMOR = "equipment_Armor";
        private const string EQUIPMENT_GLOVES = "equipment_Gloves";
        private const string EQUIPMENT_BOOTS = "equipment_Boots";
        private const string EQUIPMENT_ACCESSORY = "equipment_Accessory";

        private static ItemsContainer _itemsContainer = ItemsContainer.Instance;

        public static ItemEquipment EquipmentSword
        {
            get { return GetItem(EQUIPMENT_SWORD); }
            set { SetItem(EQUIPMENT_SWORD, value); }
        }
        public static ItemEquipment EquipmentHelmet
        {
            get { return GetItem(EQUIPMENT_HELMET); }
            set { SetItem(EQUIPMENT_HELMET, value); }
        }
        public static ItemEquipment EquipmentArmor
        {
            get { return GetItem(EQUIPMENT_ARMOR); }
            set { SetItem(EQUIPMENT_ARMOR, value); }
        }
        public static ItemEquipment EquipmentGloves
        {
            get { return GetItem(EQUIPMENT_GLOVES); }
            set { SetItem(EQUIPMENT_GLOVES, value); }
        }
        public static ItemEquipment EquipmentBoots
        {
            get { return GetItem(EQUIPMENT_BOOTS); }
            set { SetItem(EQUIPMENT_BOOTS, value); }
        }
        public static ItemEquipment EquipmentAccessory
        {
            get { return GetItem(EQUIPMENT_ACCESSORY); }
            set { SetItem(EQUIPMENT_ACCESSORY, value); }
        }

        private static ItemEquipment GetItem(string link)
        {
            string json = PlayerPrefs.GetString(link, "");
            ItemSaveData itemSaveData = JsonUtility.FromJson<ItemSaveData>(json);
            ItemEquipment itemEquipment = null;            
            if (itemSaveData != null)            
                if (itemSaveData.TryGetItemData(_itemsContainer, out ItemData itemData))
                    if (itemData.Item is ItemEquipment) itemEquipment = itemData.Item as ItemEquipment;
            
            return itemEquipment;
        }
        private static void SetItem(string link, ItemEquipment item)
        {
            ItemSaveData temp = item == null ? null : new ItemSaveData(item.ID, 1, item.RareType, item.Level);
            string json = JsonUtility.ToJson(temp);
            PlayerPrefs.SetString(link, json);
        }
    }
}