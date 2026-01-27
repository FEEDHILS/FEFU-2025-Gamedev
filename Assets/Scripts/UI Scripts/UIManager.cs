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
    public bool Locked = false;
    public GameObject InventoryCrafting, Background, FurnaceSmelting, Workbench, SleepBG;
    [SerializeField] InventoryUI InventorySlots;
    public DraggableUI DragUI;

    public static UIManager instance;
    public UnityEvent<UIState> OnStateChange;
    void Awake() => instance = this;

    void Start() => ChangeState(CurrentState);
    void LateUpdate()
    {
        if (InputSystem.actions.FindAction("OpenInventory").WasPressedThisFrame() && !Locked)
        {
            if (CurrentState == UIState.Closed)
            {
                ChangeState(UIState.Inventory);
                InventoryCrafting.SetActive(true);
            }
            else
                ChangeState(UIState.Closed);
        }

        // if (InputSystem.actions.FindAction("Interact").WasPressedThisFrame() && !Locked && CurrentState != UIState.Closed)
        //     ChangeState(UIState.Closed);
    }

    public void OpenFurnace(Furnace furnace)
    {
        ChangeState(UIState.Furnace);
        FurnaceSmelting.GetComponent<FurnaceUI>().OpenThisFurnace(furnace);
    }

    public void ChangeState(UIState state)
    {
        if (Locked)
            return;

        CurrentState = state;
        OnStateChange?.Invoke(state);

        if (state == UIState.Closed)
        {
            Background.SetActive(false);
            InventoryCrafting.SetActive(false);
            FurnaceSmelting.SetActive(false);
            Workbench.SetActive(false);
        }
        else
        {
            Background.SetActive(true);

            if (state == UIState.Furnace)
                FurnaceSmelting.SetActive(true);
            
            if (state == UIState.CraftingBench)
                Workbench.SetActive(true);
        }
    }

    public void ChangeState1(int i) => ChangeState((UIState)i);
}