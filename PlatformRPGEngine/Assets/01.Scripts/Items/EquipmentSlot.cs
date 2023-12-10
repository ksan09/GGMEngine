using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType slotType;

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = $"Equip Slot[{slotType.ToString()}]";
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if (Keyboard.current.ctrlKey.isPressed)
        {
            Inventory.Instance.equipmentWindow
                .UnEquipItem(item.itemDataSO as ItemDataEquipmentSO);
            CleanUpSlot();
        }
    }

}
