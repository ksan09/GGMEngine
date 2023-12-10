using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialStash : Stash
{
    public MaterialStash(Transform slotParentTrm) : base(slotParentTrm)
    {
    }

    public override void AddItem(ItemDataSO item, int count = 1)
    {
        if(stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack(count);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            newItem.AddStack(count - 1);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    public override void RemoveItem(ItemDataSO item, int count = 1)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem inventoryItem))
        {
            if(inventoryItem.stackSize <= count)
            {
                stash.Remove(inventoryItem);
                stashDictionary.Remove(item);
            }
            else
            {
                inventoryItem.RemoveStack(count);
            }
        }
    }

    public override bool CanAddItem(ItemDataSO itemData)
    {
        if (stash.Count >= _itemSlots.Length
            && !stashDictionary.ContainsKey(itemData))
        {
            Debug.Log("No more space in stash!");
            return false;
        }

        return true;
    }
}
