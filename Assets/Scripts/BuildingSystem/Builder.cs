using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Builder : MonoBehaviour
{
    [SerializeField] Schematic activeSchematic;

    void Start()
    {
        PlayerInventory.Instance.OnSelectedSlotChanged += SlotCheck;
        SlotCheck(PlayerInventory.Instance.SelectedSlot);
    } 


    void SlotCheck(InventorySlot slot)
    {
        if (activeSchematic)
            Destroy(activeSchematic.gameObject);

        if (slot.item && slot.item is BuildItem build)
        {
            activeSchematic = Instantiate(build.Prebuild, PlayerCursor.instance.Position, Quaternion.identity).GetComponent<Schematic>();
            activeSchematic.ItemRef = build;
            activeSchematic.OnPlaced.AddListener(Placed);
        }
    }

    void Update()
    {
        if (InputSystem.actions.FindAction("PlaceBuild").WasPressedThisFrame() && (UIManager.instance.CurrentState == UIManager.UIState.Closed))
            activeSchematic?.OnAction?.Invoke();

        // if (InputSystem.actions.FindAction("PlaceSchema").WasPressedThisFrame())
            // activeSchematic.DisableSchema();
    }

    public void Placed()
    {
        activeSchematic = null;
        PlayerInventory.Instance.RemoveAt(PlayerInventory.Instance.SelectedSlot, 1, false);
    }
}
