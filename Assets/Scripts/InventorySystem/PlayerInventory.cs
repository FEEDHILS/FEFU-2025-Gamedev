using System;
using UnityEngine;


public class PlayerInventory : Inventory
{
    public event Action<InventorySlot> OnSelectedSlotChanged;
    public InventorySlot SelectedSlot;
    // Singleton setup
    public static PlayerInventory Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        
        ChangeSelected(0); // Меняет текущий выбранный слот
        OnInventoryChange += CheckForSelected;
    }

    public void ChangeSelected(int select)
    {
        int slot = Mathf.Clamp(select, 0, Slots.Count);
        SelectedSlot = Slots[slot];

        OnSelectedSlotChanged?.Invoke(SelectedSlot);

        SelectedSlot.item?.OnEquip();
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