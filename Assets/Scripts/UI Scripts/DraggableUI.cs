using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
// using UnityEngine.InputSystem;

public class DraggableUI : MonoBehaviour
{
    public RectTransform Canvas;
    public InventorySlot Carrying = new InventorySlot();

    public Text NameUI, AmountUI;
    public GameObject ImageUI;

    public bool isDragging = false;
    public bool LockInsert = false;
    public int Amount = 1;

    // Update is called once per frame
    void Awake()
    {
        UIManager.instance.OnStateChange.AddListener((state) => { if (state == UIManager.UIState.Closed) Drop(); });
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, Mouse.current.position.ReadValue(), Canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPoint);
        if (isDragging)
            GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    // public void Interact(InventorySlot slot, Inventory Container, int button=0, bool CanInsert=true)
    // {
    //     if (!isDragging && !slot.IsEmpty)
    //     {
    //         Carrying.item = slot.item;
    //         int amount = slot.count;
            
    //         Carrying.count = slot.count;
    //         Container.RemoveAt(slot, slot.count, false);

    //         SetUIState(true);
    //     }
    //     else if(isDragging && CanInsert)
    //     {
    //         if (slot.item == Carrying.item)
    //         {
    //             Carrying.count = Container.InsertItem(slot, Carrying.count);
    //         }
    //         else
    //         {
    //             Item item = Carrying.item;
    //             int amount = Carrying.count;

    //             Carrying.item = slot.item;
    //             Carrying.count = slot.count;

    //             Container.InsertItem(slot, amount, item);
    //         }


    //         if (Carrying.IsEmpty)
    //             SetUIState(false);
    //         else
    //             SetUIState(true);
    //     }
    // }

    public void Interact(InventorySlot slot, Inventory Container, int button=0, bool CanInsert=true)
    {
        if (!isDragging && !slot.IsEmpty)
        {
            Carrying.item = slot.item;
            int amount = slot.count;
            if (button != 0)
                amount = Mathf.CeilToInt(amount / 2f);

            Carrying.count = amount;
            Container.RemoveAt(slot, amount, false);

            SetUIState(true);
        }
        else if(isDragging && CanInsert)
        {
            int amount = Carrying.count;
            if (button != 0)
                amount = 1;
            
            if (slot.item == Carrying.item)
            {
                int left = Container.InsertItem(slot, amount);
                if (left == 0)
                    Carrying.count -= amount;
                else
                    Carrying.count -= amount-left;
            }
            else
            {
                if (slot.item == null)
                {
                    Container.InsertItem(slot, amount, Carrying.item);
                    Carrying.Remove(amount);
                }
                else
                {
                    Item item = Carrying.item;
                    amount = Carrying.count;

                    Carrying.item = slot.item;
                    Carrying.count = slot.count;

                    Container.InsertItem(slot, amount, item);
                }

            }


            if (Carrying.IsEmpty)
                SetUIState(false);
            else
                SetUIState(true);
        }
    }

    public void Drop()
    {
        int left = 0;
        if (isDragging)
            left = PlayerInventory.Instance.AddItem(Carrying.item, Carrying.count);

        if (left != 0)
            DropManager.Instance.Drop(Carrying.item, left, PlayerInventory.Instance.transform.position, Quaternion.identity);


        Carrying.item = null;
        Carrying.count = 0;
        SetUIState(false);
    }

    void SetUIState(bool state)
    {
        NameUI.gameObject.SetActive(state);
        AmountUI.gameObject.SetActive(state);
        ImageUI.SetActive(state);
        isDragging = state;

        if (state)
        {
            NameUI.text = Carrying.item.itemName;
            AmountUI.text = Carrying.count > 1 ? Carrying.count.ToString() : "";
        }
        else
        {
            NameUI.text = "";
            AmountUI.text = "";
        }
    }
}
