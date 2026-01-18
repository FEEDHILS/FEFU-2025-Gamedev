using PlasticGui.WorkspaceWindow.Merge;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ViewmodelController : MonoBehaviour
{
    public GameObject Current = null;
    public UnityEvent OnEquip;
    public UnityEvent OnDiscard;
    void Start()
    {
        PlayerInventory.Instance.OnSelectedSlotChanged += ItemCheck;
    }

    void ItemCheck(InventorySlot selected)
    {
        if (Current)
        {
            Destroy(Current);
            OnDiscard.Invoke();
        }


        if (selected.item && selected.item is ToolItem tool)
        {
            Current = Instantiate(tool.Viewmodel, transform);
            OnEquip.Invoke();
        }
    }

    void Update()
    {
        if (!Current) return;
        Current.TryGetComponent<ViewmodelAnimator>(out ViewmodelAnimator handler);

        if (InputSystem.actions.FindAction("Attack").IsPressed() && (UIManager.instance.CurrentState == UIManager.UIState.Closed))
        {
            handler.PrimaryAction();
        }
    }
}
