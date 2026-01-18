using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        Closed,
        Inventory,
        CraftingBench,
        Furnace,
        Chest
    }

    public UIState CurrentState;
    [SerializeField] GameObject InventoryCrafting, Background;
    [SerializeField] InventoryUI InventorySlots;
    public DraggableUI DragUI;

    public static UIManager instance;
    public UnityEvent<UIState> OnStateChange;
    void Awake() => instance = this;

    void Start() => ChangeState(CurrentState);
    void Update()
    {
        if (InputSystem.actions.FindAction("OpenInventory").WasPressedThisFrame())
        {
            if (CurrentState == UIState.Closed)
            {
                ChangeState(UIState.Inventory);
                InventoryCrafting.SetActive(true);
            }
            else
                ChangeState(UIState.Closed);
        }
    }

    public void ChangeState(UIState state)
    {
        CurrentState = state;
        OnStateChange?.Invoke(state);

        if (state == UIState.Closed)
        {
            Background.SetActive(false);
            InventoryCrafting.SetActive(false);
        }
        else
        {
            Background.SetActive(true);
        }
    }

    public void ChangeState1(int i) => ChangeState((UIState)i);
}