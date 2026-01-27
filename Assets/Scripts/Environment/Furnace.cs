using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Furnace : Inventory
{
    public InventorySlot BurnSlot, FuelSlot, ReadySlot;
    [System.Serializable]
    class Burnable
    {
        public Item item;
        public int amount = 1;
    }
    [System.Serializable]
    class SmeltRecipe
    {
        public Item Smeltable;
        public Item Result;
        public float ratio = 1; // На будущее быть может
    }
    [Space()]
    [SerializeField] Burnable[] BurnItems;
    [SerializeField] SmeltRecipe[] Recipes;
    void Awake()
    {
        BurnSlot = Slots[0];
        FuelSlot = Slots[1];
        ReadySlot = Slots[2];

        OnInventoryChange += RecipeCheck;
    }

    SmeltRecipe currentRecipe = null;
    void RecipeCheck(InventorySlot slot)
    {
        foreach (SmeltRecipe i in Recipes)
        {
            if (BurnSlot.item == i.Smeltable)
            {
                currentRecipe = i;
                BurnFuel();
                return;
            }
        }
        currentRecipe = null;
        burnProgress = 0;
    }

    [SerializeField] float burnTime = 0;
    void BurnFuel()
    {
        if (currentRecipe != null)
        {
            Burnable current = BurnItems.FirstOrDefault(x => x.item == FuelSlot.item);

            if (current == null || (ReadySlot.item != null && ReadySlot.item != currentRecipe.Result) || (ReadySlot.item != null && ReadySlot.IsFull) || burnTime > 0)
                return;

            burnTime = current.amount;
            RemoveAt(FuelSlot, 1, false);
        }
    }

    [Range(0f, 10f)]
    public float burnProgress = 0f;
    void Update()
    {
        if (burnTime > 0 && currentRecipe != null)
        {
            burnProgress = Mathf.Clamp(burnProgress + Time.deltaTime, 0, 10);

            if (burnProgress >= 10)
            {
                burnTime -= 1;
                burnProgress = 0;
                if (ReadySlot.IsEmpty)
                    ReadySlot.Add(1, currentRecipe.Result);
                else
                    ReadySlot.Add(1);
                
                RemoveAt(BurnSlot, 1, false);
                BurnFuel();
            }
        }
    }

    public void EnableUI()
    {
        UIManager.instance.OpenFurnace(this);
    }
}
