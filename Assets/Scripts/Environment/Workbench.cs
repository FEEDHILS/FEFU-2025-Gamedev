using UnityEngine;

public class Workbench : MonoBehaviour
{
    public void OpenWorkbench() => UIManager.instance.ChangeState(UIManager.UIState.CraftingBench);
}
