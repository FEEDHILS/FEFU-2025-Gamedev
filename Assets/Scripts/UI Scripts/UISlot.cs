using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class UISlot : MonoBehaviour
{
    public InventorySlot Slot = null;

    public Text ItemName, ItemAmount;
    public void Init()
    {
        PlayerInventory.Instance.OnInventoryChange += UpdateUI;
        UpdateUI(Slot);
    }

    void UpdateUI(InventorySlot _slot)
    {
        if (Slot.item != null)
        {
            ItemName.text = Slot.item.itemName;
            ItemAmount.text = Slot.count > 1 ? Slot.count.ToString() : "";
        }
        else
        {
            ItemName.text = "";
            ItemAmount.text = "";
        }
    }

    public void Dragging()
    {
        DraggableUI ui = InventoryUI.Instance.DragUI;

        ui.Interact(Slot);
    }
}
