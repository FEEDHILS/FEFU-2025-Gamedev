using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public bool State = false;

    [Header("Slots Settings (Must be set!)")]
    public int StartIndex = 4;
    public UISlot[] UISlots;

    [Space(16)]
    [Header("Global References")]
    public CameraController CursorState;
    public DraggableUI DragUI;
    public static InventoryUI Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < UISlots.Length; i++)
        {
            UISlots[i].Slot = PlayerInventory.Instance.Slots[i + StartIndex];
            UISlots[i].Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.actions.FindAction("OpenInventory").WasPressedThisFrame())
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        State = !State;
        if (State)
            CursorState.MouseUnLock();
        else
        {
            CursorState.MouseLock();
            DragUI.Drop();
        }

        transform.GetChild(0).gameObject.SetActive(State);
    }
}
