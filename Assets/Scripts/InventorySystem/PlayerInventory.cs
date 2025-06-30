using System;
using System.Collections.Generic;
using System.Collections;

using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Codice.Client.GameUI.Explorer;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> Slots = new List<InventorySlot>(20);
    public InventorySlot SelectedSlot = null;

    public event Action<InventorySlot> OnSelectedSlotChanged;
    public event Action<InventorySlot> OnInventoryChange;

    // Singleton setup
    public static PlayerInventory Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        OnInventoryChange += CheckForSelected;
        ChangeSelected(0); // Меняет текущий выбранный слот
    }

    public void ChangeSelected(int select)
    {
        int slot = Mathf.Clamp(select, 0, Slots.Count);
        SelectedSlot = Slots[slot];

        OnSelectedSlotChanged?.Invoke(SelectedSlot);

        SelectedSlot.item?.OnEquip();
    }

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

        Debug.LogError("Could subtract Sufficient amount of Item: " + Item.name + ", left: " + Value);
    }

    public void RemoveAt(InventorySlot slot, int amount = 1, bool DoDrop = true)
    {
        if (slot.IsEmpty) return;

        if (DoDrop)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            DropManager.Instance.Drop(slot.item, amount, transform.position, camera.transform.rotation, 5);
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

    // public void ManualInventoryChange(InventorySlot slot) => OnInventoryChange?.Invoke(slot); // Obsolete maybe?
    public void CheckForSelected(InventorySlot slot)
    {
        // Иногда изменения в инвенторе влияют на текущий выбранный слот в хотбаре,
        // Если это произошло, то не забываем активировать соответствующий ивент.
        if (slot == SelectedSlot)
        {
            OnSelectedSlotChanged?.Invoke(SelectedSlot);  
            SelectedSlot.item?.OnEquip();
        }
    } 
}