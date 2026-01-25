using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public Inventory Container;
    public InventorySlot Slot = null;
    [SerializeField] bool CanInsert = true;

    public Text ItemName, ItemAmount;
    public void Init()
    {
        // Сначала отписываемся на случай, если Init вызван повторно
        if (Container != null) 
             Container.OnInventoryChange -= UpdateUI;

        Container.OnInventoryChange += UpdateUI;
        UpdateUI(Slot);
    }

    void UpdateUI(InventorySlot _)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        DraggableUI ui = UIManager.instance.DragUI;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ui.Interact(Slot, Container, 0, CanInsert);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ui.Interact(Slot, Container, 1, CanInsert);
        }
    }
}
