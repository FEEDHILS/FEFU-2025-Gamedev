using System.Collections.Generic;
using UnityEngine;

public class CraftingLogic : MonoBehaviour
{
    public List<CraftingRecipe> Recipies = new List<CraftingRecipe>();
    public GameObject UIParent;
    public Vector3 StartingPos = new Vector3(10, 100);
    public Vector3 Increment = new Vector3(0, -90);

    public GameObject Prefab;

    private Dictionary<RecipeUI, CraftingRecipe> Map = new();

    public CraftingRecipe CurrentRecipe;

    void Awake()
    {
        PopulateUI();
    }

    void PopulateUI()
    {
        for (int i = 0; i < Recipies.Count; i++)
        {
            GameObject instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity, UIParent.transform);

            RectTransform kek = instance.GetComponent<RectTransform>();

            kek.anchoredPosition3D = StartingPos + Increment * i;
            kek.localEulerAngles = Vector3.zero;

            instance.GetComponent<RecipeUI>().recipe = Recipies[i];
            instance.GetComponent<RecipeUI>().Manager = this;
            instance.GetComponent<RecipeUI>().Init();
        }
    }

    public void UIHide()
    {
        CurrentRecipe = null;
    }

    public void SetSelectedRecipe(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;
        print("Selected Recipe " + recipe.Name);
    }

    public void Craft()
    {
        if (!CurrentRecipe) return;

        bool CanCraft = true;
        foreach (InventorySlot i in CurrentRecipe.Ingridients)
        {
            if (PlayerInventory.Instance.CountItem(i.item) < i.count)
                CanCraft = false;

        }

        if (CanCraft)
        {
            PlayerInventory.Instance.AddItem(CurrentRecipe.CraftableItem, CurrentRecipe.Amount);
            foreach (InventorySlot i in CurrentRecipe.Ingridients)
                PlayerInventory.Instance.RemoveItem(i.item, i.count);
        }
        else
            print("Couldn't Craft " + CurrentRecipe.Name);
    }
}