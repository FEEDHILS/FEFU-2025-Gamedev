using System.Collections.Generic;
using UnityEngine;

public class UIHotbarController : MonoBehaviour
{
    public UISlot[] UISlots;
    private Dictionary<InventorySlot, UISlot> slotMap = new();
    public RectTransform SelectionUI;
    void Start()
    {
        for (int i = 0; i < UISlots.Length; i++)
        {
            InventorySlot slot = PlayerInventory.Instance.Slots[i];
            UISlots[i].Slot = slot;
            UISlots[i].Container = PlayerInventory.Instance;
            UISlots[i].Init();

            slotMap[slot] = UISlots[i];
        }


        if (SelectionUI is not null)
            PlayerInventory.Instance.OnSelectedSlotChanged += UpdateUI;
    }

    // Update is called once per frame
    void UpdateUI(InventorySlot selected)
    {
        float yPos = SelectionUI.anchoredPosition.y;

        SelectionUI.anchoredPosition = new Vector2(slotMap[selected].GetComponent<RectTransform>().anchoredPosition.x, yPos);
    }
    
}
