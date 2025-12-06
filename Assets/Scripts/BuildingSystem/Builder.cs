using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Builder : MonoBehaviour
{
    GameObject activeSchematic;
    // Lame Singleton
    public static Builder instance = null;
    void OnEnable() => instance = this;
    void OnDisable() => instance = null;

    void Awake() => PlayerInventory.Instance.OnSelectedSlotChanged += SlotCheck;


    void SlotCheck(InventorySlot slot)
    {
        if (activeSchematic)
            Destroy(activeSchematic);

        if (slot.item && slot.item is BuildItem build)
        {
            activeSchematic = Instantiate(build.Prebuild, Vector3.zero, Quaternion.identity);
        }
    }

    void Update()
    {
        if (activeSchematic && InputSystem.actions.FindAction("Place").WasPressedThisFrame())
        {
            Schematic schematic = activeSchematic.GetComponentInChildren<Schematic>();
            if (schematic)
                schematic.OnAction.Invoke();
        }
    }

    public void Placed()
    {
        activeSchematic = null;
        PlayerInventory.Instance.RemoveAt(PlayerInventory.Instance.SelectedSlot, 1, false);
    }
}
