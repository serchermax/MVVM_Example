using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class EquipmentModel
{
    private InventoryVM _inventoryVM;
    public EquipmentModel(InventoryVM inventoryVM)
    {
        _inventoryVM = inventoryVM;
        _equipmnet = new Dictionary<EquipmentTypes, ItemEquipment>();

        for (int i = 0; i < 6; i++)
            _equipmnet.Add((EquipmentTypes)i, GetDataEquipment((EquipmentTypes)i));
    }
    private Dictionary<EquipmentTypes, ItemEquipment> _equipmnet;

    public ItemEquipment ReplaceEquipment(ItemEquipment replaceItem)
    {
        SetEquipment(replaceItem.EquipmentType, replaceItem);
        TryGetEquipment(replaceItem.EquipmentType, out ItemEquipment item);
        return item;
    }

    public bool TryChangeEquipment(ItemData itemToEquip, out ItemEquipment equipment)
    {
        equipment = null;
        if (_inventoryVM.TryGetItemDataOfType<ItemEquipment>(itemToEquip))
        {
            _inventoryVM.RemoveItem(itemToEquip);
            ItemEquipment item = itemToEquip.Item as ItemEquipment;
            equipment = ChangeEquipment(item.EquipmentType, item);          
        } 
        return equipment != null;
    }
    public bool TryChangeEquipment(int newItemIndex, out ItemEquipment equipment)
    {
        equipment = null;
        if (_inventoryVM.TryGetItemDataOfType<ItemEquipment>(newItemIndex, out ItemData itemData)) 
        {
            _inventoryVM.RemoveItem(newItemIndex);
            ItemEquipment item = itemData.Item as ItemEquipment;
            equipment = ChangeEquipment(item.EquipmentType, item);
        }        
        return equipment != null;
    }

    private ItemEquipment ChangeEquipment(EquipmentTypes itemType, ItemEquipment item)
    {
        if (TryGetEquipment(itemType, out ItemEquipment oldItem))
        {
            _inventoryVM.AddItem(oldItem);
            SetEquipment(itemType, null);
        }       
        SetEquipment(itemType, item);
        return item;
    }

    public virtual void RemoveEquipment(EquipmentTypes itemType)
    {
        if (TryGetEquipment(itemType, out ItemEquipment oldItem))
        {
            _inventoryVM.AddItem(oldItem);
        }
        SetEquipment(itemType, null);
    }
    
    public bool TryGetBoosters(out ItemStatBooster[] boosters)
    {
        List<ItemStatBooster> temp = new List<ItemStatBooster>();

        int max = (int)EquipmentTypes.Accessory;        
        for (int i = 0; i < max; i++)
        {
            if (TryGetEquipment((EquipmentTypes)i, out ItemEquipment item))
            {
                if (item.ItemBoost != null)
                    temp.AddRange(item.ItemBoost);
            }
        }
        boosters = temp.ToArray();
        return temp.Count > 0;
    }

    public bool TryGetEquipment(EquipmentTypes itemType, out ItemEquipment item)
    {
        item = _equipmnet[itemType];  
        return item != null;
    }

    public void SetEquipment(EquipmentTypes itemType, ItemEquipment item)
    {
        _equipmnet[itemType] = item;
        SetDataEquipment(itemType, item);
    }

    private ItemEquipment GetDataEquipment(EquipmentTypes itemType)
    {
        switch (itemType)
        {
            case EquipmentTypes.Sword: return EquipmentData.EquipmentSword; 
            case EquipmentTypes.Helmet: return EquipmentData.EquipmentHelmet; 
            case EquipmentTypes.Armor: return EquipmentData.EquipmentArmor; 
            case EquipmentTypes.Gloves: return EquipmentData.EquipmentGloves; 
            case EquipmentTypes.Boots: return EquipmentData.EquipmentBoots; 
            case EquipmentTypes.Accessory: return EquipmentData.EquipmentAccessory;
            default: return null;
        }
    }
    private void SetDataEquipment(EquipmentTypes itemType, ItemEquipment item)
    {
        switch (itemType)
        {
            case EquipmentTypes.Sword: EquipmentData.EquipmentSword = item; break;
            case EquipmentTypes.Helmet: EquipmentData.EquipmentHelmet = item; break;
            case EquipmentTypes.Armor: EquipmentData.EquipmentArmor = item; break;
            case EquipmentTypes.Gloves: EquipmentData.EquipmentGloves = item; break;
            case EquipmentTypes.Boots: EquipmentData.EquipmentBoots = item; break;
            case EquipmentTypes.Accessory: EquipmentData.EquipmentAccessory = item; break;
        }
    }
    public void SaveTempToData(EquipmentTypes type) => SetDataEquipment(type, _equipmnet[type]);
}
