using UnityEngine;
using UnityEngine.UI;

public class FurnaceUI : MonoBehaviour
{
    public UISlot[] UISlots;
    public Slider SmeltingProgress;

    Furnace current;
    void Awake()
    {
        // UIManager.instance.OnStateChange.AddListener(ChangeState);
    }

    public void OpenThisFurnace(Furnace furnace)
    {
        for (int i = 0; i < UISlots.Length; i++)
        {
            UISlots[i].Slot = furnace.Slots[i];
            UISlots[i].Container = furnace;
            UISlots[i].Init();
        }
        current = furnace;
    }

    void Update()
    {
        SmeltingProgress.value = current.burnProgress / 10;
    }

    // public void ChangeState(UIManager.UIState state)
    // {
    //     if (state == UIManager.UIState.Furnace)
    //     {
            
    //     }
    // }
}
