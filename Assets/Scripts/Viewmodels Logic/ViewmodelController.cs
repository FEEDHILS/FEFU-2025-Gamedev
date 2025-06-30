using PlasticGui.WorkspaceWindow.Merge;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewmodelController : MonoBehaviour
{
    GameObject Current = null;
    void Start()
    {
        PlayerInventory.Instance.OnSelectedSlotChanged += ItemCheck;
    }

    // Update is called once per frame
    void ItemCheck(InventorySlot selected)
    {
        if (Current)
        {
            Destroy(Current);
        }


        if (selected.item && selected.item is ToolItem tool)
        {
            Current = Instantiate(tool.Viewmodel, transform);
        }
    }

    void Update()
    {
        if (!Current) return;
        Current.TryGetComponent<ViewmodelHandler>(out ViewmodelHandler handler);

        if (InputSystem.actions.FindAction("Attack").IsPressed() && !InventoryUI.Instance.State)
        {
            handler.ChangeState(ViewmodelStates.Attack);
        }
    }
}
