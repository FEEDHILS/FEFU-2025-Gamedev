using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    [SerializeField] Slider UI;
    [SerializeField] float PerTick = 0.01f;
    [SerializeField] float Hunger = 100f;
    [SerializeField] string DeathScene;
    void Start()
    {
        PlayerInventory.Instance.OnSelectedSlotChanged += SlotCheck;
        SlotCheck(PlayerInventory.Instance.SelectedSlot);
    } 

    InventorySlot selectedFood;
    void SlotCheck(InventorySlot slot)
    {
        selectedFood = slot;
    }

    
    void Update()
    {
        Hunger -= PerTick * Time.deltaTime;
        bool outUI = UIManager.instance.CurrentState == UIManager.UIState.Closed;
        if (selectedFood != null && selectedFood.item is FoodItem food && InputSystem.actions.FindAction("Attack").WasPressedThisFrame() && outUI)
        {
            Hunger += food.Saturation;

            PlayerInventory.Instance.RemoveAt(selectedFood, 1, false);
        }

        Hunger = Mathf.Clamp(Hunger, 0, 100);
        UI.value = Hunger;

        if (Hunger < 1)
            PlayerDeath();
    }


    void PlayerDeath()
    {
        SceneManager.LoadScene(DeathScene);
    }
}
