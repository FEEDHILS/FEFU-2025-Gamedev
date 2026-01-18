using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("Slots Settings (Must be set!)")]
    public int StartIndex = 4;
    public UISlot[] UISlots;

    [Space(16)]
    [Header("Global References")]
    public CameraController CursorState;
    // public DraggableUI DragUI;

    void Awake() => UIManager.instance.OnStateChange.AddListener(ChangeState);

    void Start()
    {
        for (int i = 0; i < UISlots.Length; i++)
        {
            UISlots[i].Slot = PlayerInventory.Instance.Slots[i + StartIndex];
            UISlots[i].Init();
        }
    }

    public void ChangeState(UIManager.UIState state)
    {
        if (state != UIManager.UIState.Closed)
        {
            CursorState.MouseUnlock();
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            CursorState.MouseLock();
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
