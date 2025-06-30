using System;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int count = 0;

    public bool IsEmpty => count == 0;
    public bool IsFull => count == item.maxStackSize;

    public void Add(int value, Item Item = null)
    {
        if (Item is not null)
        {
            item = Item;
            count = value;
            return;
        }    
        if (item is null) Debug.LogError("Trying to Add to Null-Item Slot");

        count = Mathf.Min(item.maxStackSize, count + value);
    }

    public void Remove(int value)
    {
        if (item is null) Debug.LogError("Trying to Remove from Null-Item Slot");
        count = Mathf.Max(0, count - value);

        if (count == 0) item = null;
    }
}