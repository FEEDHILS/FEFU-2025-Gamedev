using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingController : MonoBehaviour
{

    BuildManager previous;
    void Start()
    {
        PlayerInventory.Instance.OnSelectedSlotChanged += SlotCheck;
    }


    void SlotCheck(InventorySlot slot)
    {
        if (previous)
            Destroy(previous.gameObject);

        if (slot.item && slot.item is BuildItem build)
        {
            GameObject instance = Instantiate(build.Prebuild, PlayerCursor.Position, Quaternion.identity);

            if (!instance.TryGetComponent<BuildManager>(out previous))
            {
                Debug.LogError("Cant get BuildManager from Building");
            }

            previous.OnBuildPlaced.AddListener(Placed);
        }
    }

    void Update()
    {
        if (previous && InputSystem.actions.FindAction("Place").WasPressedThisFrame())
        {
            previous.Place();
        }
    }

    void Placed()
    {
        previous = null;
        PlayerInventory.Instance.RemoveAt(PlayerInventory.Instance.SelectedSlot, 1, false);
    }
}
