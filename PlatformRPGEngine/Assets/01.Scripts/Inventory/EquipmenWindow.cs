using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentWindow
{
    public List<InventoryItem> equipments;
    public Dictionary<ItemDataEquipmentSO, InventoryItem> equipmentDictionary;

    protected Transform _parentTrm;
    protected EquipmentSlot[] _equipmentSlots;

    public EquipmentWindow(Transform parentTrm)
    {
        _parentTrm = parentTrm;

        equipments = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipmentSO, InventoryItem>();

        _equipmentSlots = parentTrm.GetComponentsInChildren<EquipmentSlot>();
    }

    public void UpdateSlotUI()
    {
        for(int i = 0; i < _equipmentSlots.Length; ++i)
        {
            EquipmentSlot currentSlot = _equipmentSlots[i];
            ItemDataEquipmentSO equipment 
                = equipmentDictionary.Keys.ToList().Find(x => x.equipmentType == currentSlot.slotType);

            if(equipment != null)
            {
                currentSlot.UpdateSlot(equipmentDictionary[equipment]);
            }
            else
            {
                currentSlot.CleanUpSlot();
            }
        }
    }

    public ItemDataEquipmentSO GetEquipmentByType(EquipmentType type)
    {
        ItemDataEquipmentSO targetItem = null;

        foreach(var pair in equipmentDictionary)
        {
            if(pair.Key.equipmentType == type)
            {
                targetItem = pair.Key;
                break;
            }
        }

        return targetItem;
    }

    public void EquipItem(ItemDataEquipmentSO equipment)
    {
        InventoryItem newItem = new InventoryItem(equipment);

        ItemDataEquipmentSO oldEquipment = GetEquipmentByType(equipment.equipmentType);
        if(oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
        }

        equipments.Add(newItem);
        equipmentDictionary.Add(equipment, newItem);
        equipment.AddModifers();
    }

    public void UnEquipItem(ItemDataEquipmentSO oldEquipment)
    {
        if (!equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem oldItem))
            return;

        equipments.Remove(oldItem);
        equipmentDictionary.Remove(oldEquipment);
        oldEquipment.RemoveModifiers();

        Inventory.Instance.AddItem(oldEquipment);
    }
}
