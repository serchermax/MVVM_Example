using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentVM : IDisposable
{
    public delegate void EquipListenerDelegate(ItemEquipment item);
    private Dictionary<EquipmentTypes, EquipListenerDelegate> _onEquipChanged;
    public event Action OnEquipmentChanged;

    public static EquipmentVM Instance
    {
        get { return _instance == null ? new EquipmentVM() : _instance; }
    }
    private static EquipmentVM _instance;

    private InventoryVM _inventoryVM;
    private EquipmentModel _equipmentModel;
    private EquipmentVM()
    {
        _instance = this;
        _inventoryVM = InventoryVM.Instance;
        _equipmentModel = new EquipmentModel(_inventoryVM);
        _onEquipChanged = new Dictionary<EquipmentTypes, EquipListenerDelegate>();

        Item.OnItemChanged += ItemChanged;
    }

    public bool TryGetEquipment(EquipmentTypes itemType, out ItemEquipment item)
    {
        return _equipmentModel.TryGetEquipment(itemType, out item);
    }
    public bool TryGetBoosters(out ItemStatBooster[] boosters) => _equipmentModel.TryGetBoosters(out boosters);

    public bool TryChangeEquipment(int newItemIndex)
    {
        if (_equipmentModel.TryChangeEquipment(newItemIndex, out ItemEquipment equipment))
        {
            if (_onEquipChanged.ContainsKey(equipment.EquipmentType)) _onEquipChanged[equipment.EquipmentType]?.Invoke(equipment);
            OnEquipmentChanged?.Invoke();
        }
        return equipment != null;
    }
    public bool TryChangeEquipment(ItemData itemToEquip)
    {        
        if (_equipmentModel.TryChangeEquipment(itemToEquip, out ItemEquipment equipment))
        {
            if (_onEquipChanged.ContainsKey(equipment.EquipmentType)) _onEquipChanged[equipment.EquipmentType]?.Invoke(equipment);
            OnEquipmentChanged?.Invoke();
        }
        return equipment != null;
    }

    public virtual bool TryRemoveEquipment(Item item)
    {
        if (item is ItemEquipment)
        {
            RemoveEquipment(item as ItemEquipment);
            return true;
        }
        return false;
    }
    public virtual void RemoveEquipment(ItemEquipment item) => RemoveEquipment(item.EquipmentType); 
    public virtual void RemoveEquipment(EquipmentTypes itemType)
    {
        _equipmentModel.RemoveEquipment(itemType);
        if (_onEquipChanged.ContainsKey(itemType)) _onEquipChanged[itemType]?.Invoke(null);
        OnEquipmentChanged?.Invoke();
    }

    public void AddListener(EquipListenerDelegate itemListenerDelegate, EquipmentTypes itemType)
    {
        if (_onEquipChanged.ContainsKey(itemType))
        {
            _onEquipChanged[itemType] += itemListenerDelegate;
        }
        else _onEquipChanged.Add(itemType, itemListenerDelegate);
    }
    public void RemoveListener(EquipListenerDelegate itemListenerDelegate, EquipmentTypes itemType)
    {
        if (_onEquipChanged.ContainsKey(itemType))
        {
            _onEquipChanged[itemType] -= itemListenerDelegate;
        }
    }

    public void Dispose() => Item.OnItemChanged -= ItemChanged;

    private void ItemChanged(Item item)
    {
        if (item.ItemCategory == ItemCategories.Equipments)
            if (TryGetEquipment((item as ItemEquipment).EquipmentType, out ItemEquipment equip))
                if (equip == item)
                    _equipmentModel.SaveTempToData(equip.EquipmentType);
    }
}
