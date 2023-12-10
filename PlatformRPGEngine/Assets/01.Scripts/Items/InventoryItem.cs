using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class InventoryItem
{
    public ItemDataSO itemDataSO;
    public int stackSize;

    public InventoryItem(ItemDataSO itemDataSO)
    {
        this.itemDataSO = itemDataSO;
        AddStack();
    }

    public void AddStack(int count = 1)
    {
        stackSize += count;
    }

    public void RemoveStack(int count)
    {
        stackSize -= count;
    }
}
