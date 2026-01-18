using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Create new Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string Name;
    public InventorySlot[] Ingridients;

    public string Description;
    public Item CraftableItem;
    public int Amount = 1;
}