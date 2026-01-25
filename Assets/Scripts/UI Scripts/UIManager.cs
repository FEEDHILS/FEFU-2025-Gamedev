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
    [SerializeField] GameObject InventoryCrafting, Background, FurnaceSmelting;
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

    public void OpenFurnace(Furnace furnace)
    {
        ChangeState(UIState.Furnace);
        FurnaceSmelting.SetActive(true);
        FurnaceSmelting.GetComponent<FurnaceUI>().OpenThisFurnace(furnace);
    }

    public void ChangeState(UIState state)
    {
        CurrentState = state;
        OnStateChange?.Invoke(state);

        if (state == UIState.Closed)
        {
            Background.SetActive(false);
            InventoryCrafting.SetActive(false);
            FurnaceSmelting.SetActive(false);
        }
        else
        {
            Background.SetActive(true);

            if (state == UIState.Furnace)
                FurnaceSmelting.SetActive(true);
        }
    }

    public void ChangeState1(int i) => ChangeState((UIState)i);
}