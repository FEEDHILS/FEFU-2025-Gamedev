using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> Slots = new List<InventorySlot>(20);
    public event Action<InventorySlot> OnInventoryChange;

    // Singleton setup
    // public static PlayerInventory Instance { get; private set; }

    public int AddItem(Item Item, int Value)
    {
        List<InventorySlot> EmptySlots = new List<InventorySlot>(20);

        foreach (InventorySlot i in Slots)
        {
            if (i.IsEmpty) EmptySlots.Add(i);

            if (i.item == Item && !i.IsFull)
            {
                Value = InsertItem(i, Value);
            }

            if (Value == 0) return 0;
        }

        foreach (InventorySlot j in EmptySlots)
        {
            Value = InsertItem(j, Value, Item);
            if (Value == 0) return 0;
        }


        return Value; // Остаток, если удалось разнести все предметы по ячейкам
    }

    [ContextMenu("Trigger Inv. Change")]
    void TriggerChange()
    {
        OnInventoryChange?.Invoke(null);
    }
    
    // Не указывая Item мы делаем добавку. Если указать Item - произойдет перезапись (Прошлое значение полностью пропадет)
    public int InsertItem(InventorySlot slot, int Value, Item Item = null)
    {
        int amount;

        if (!Item)
            amount = Mathf.Min(Value, slot.item.maxStackSize - slot.count);
        else
            amount = Mathf.Min(Value, Item.maxStackSize);


        slot.Add(amount, Item);

        OnInventoryChange?.Invoke(slot);

        Value -= amount;
        return Value;
    }

    // Полезно при уничтожении контейнера
    public void DropEverything()
    {
        foreach (InventorySlot slot in Slots)
        {
            if (!slot.IsEmpty)
                DropManager.Instance.Drop(slot.item, slot.count, transform.position, Quaternion.identity);
        }
    }

    // Просто ищем и удаляем n-ое кол-во предмета
    public void RemoveItem(Item Item, int Value)
    {
        foreach (InventorySlot i in Slots)
        {
            if (i.item == Item)
            {
                int amount = Mathf.Min(Value, i.count);
                i.Remove(amount);
                Value -= amount;

                OnInventoryChange?.Invoke(i);
            }

            if (Value == 0) return;
        }

        Debug.LogError("Couldn't subtract sufficient amount of Item: " + Item.name + ", left: " + Value);
    }

    public void RemoveAt(InventorySlot slot, int amount = 1, bool DoDrop = true)
    {
        // if (slot.IsEmpty) return;

        if (DoDrop && !(slot.item == null))
        {
            Vector3 DropDirection = (PlayerCursor.instance.Position - PlayerCursor.instance.Anchor.position).normalized;

            DropManager.Instance.Drop(slot.item, amount, PlayerCursor.instance.transform.position, Quaternion.LookRotation(DropDirection), DropDirection);
        }

        slot.Remove(amount);
        OnInventoryChange?.Invoke(slot);
    }

    public int CountItem(Item Item)
    {
        int total = 0;
        foreach (InventorySlot i in Slots)
        {
            if (i.item == Item)
            {
                total += i.count;
            }
        }

        return total;
    }
}