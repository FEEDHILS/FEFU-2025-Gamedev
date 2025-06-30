using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    public CraftingLogic Manager;
    public CraftingRecipe recipe;
    public Text NameUI;

    public void Init()
    {
        NameUI.text = recipe.Name;
    }

    public void Select()
    {
        Manager.SetSelectedRecipe(recipe);
    }
}