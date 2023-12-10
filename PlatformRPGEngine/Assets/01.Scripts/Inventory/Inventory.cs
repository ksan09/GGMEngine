using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public MaterialStash materialStash;
    public EquipmentStash equipmentStash;
    public EquipmentWindow equipmentWindow;

    [Header("ParentTrm")]
    [SerializeField] private Transform _materialStashTrm;
    [SerializeField] private Transform _equipmentStashTrm;
    [SerializeField] private Transform _equipmentsTrm;

    private void Awake()
    {
        materialStash = new MaterialStash(_materialStashTrm);
        equipmentStash = new EquipmentStash(_equipmentStashTrm);
        equipmentWindow = new EquipmentWindow(_equipmentsTrm);
    }

    private void Start()
    {
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        materialStash.UpdateSlotUI();
        equipmentStash.UpdateSlotUI();
        equipmentWindow.UpdateSlotUI();
    }

    public void AddItem(ItemDataSO item, int count = 1)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                if (materialStash.CanAddItem(item))
                {
                    materialStash.AddItem(item, count);
                }
                break;
            case ItemType.Equipment:
                if (equipmentStash.CanAddItem(item))
                {
                    equipmentStash.AddItem(item, count);
                }
                break;
            default:
                break;
        }

        UpdateSlotUI();
    }

    public void RemoveItem(ItemDataSO item, int count = 1)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                materialStash.RemoveItem(item, count);
                break;
            case ItemType.Equipment:
                equipmentStash.RemoveItem(item, count);
                break;
            default:
                break;
        }

        UpdateSlotUI();
    }
}
