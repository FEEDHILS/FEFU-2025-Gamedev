using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class CraftingLogic : MonoBehaviour
{
    public List<CraftingRecipe> Recipies = new List<CraftingRecipe>();
    public GameObject UIRecipeList, UIIngridientList, UIDescription;
    public Vector3 StartingPosRecipe = new Vector3(10, 100);
    public Vector3 StartingPosIngridient = new Vector3(0, 100);
    public Vector3 Increment = new Vector3(0, -90);

    public GameObject RecipeUIPrefab, IngridientUIPrefab;

    public CraftingRecipe CurrentRecipe;
    // private Dictionary<RecipeUI, CraftingRecipe> Map = new();
    List<GameObject> recipies = new List<GameObject>();

    void Awake()
    {
        PopulateUI();
        UIManager.instance.OnStateChange.AddListener( (x) => { if (x != UIManager.UIState.Inventory) ClearSelected(); } );
    }

    [ContextMenu("Populate With Recipies")]
    void PopulateUI()
    {
        for (int i = 0; i < Recipies.Count; i++)
        {
            GameObject instance = Instantiate(RecipeUIPrefab, Vector3.zero, Quaternion.identity, UIRecipeList.transform);

            RectTransform kek = instance.GetComponent<RectTransform>();

            kek.anchoredPosition3D = StartingPosRecipe + Increment * i;
            kek.localEulerAngles = Vector3.zero;

            instance.GetComponent<RecipeUI>().recipe = Recipies[i];
            instance.GetComponent<RecipeUI>().Manager = this;
            instance.GetComponent<RecipeUI>().Init();
            instance.GetComponent<Toggle>().group = UIRecipeList.GetComponent<ToggleGroup>();

            recipies.Add(instance);
        }
    }

    public void ClearSelected()
    {
        CurrentRecipe = null;
        UIDescription.GetComponent<TextMeshProUGUI>().text = "";
        UIRecipeList.GetComponent<ToggleGroup>().SetAllTogglesOff();


        foreach (Transform child in UIIngridientList.transform) 
            Destroy(child.gameObject);
    }

    public void SetSelectedRecipe(CraftingRecipe recipe)
    {
        ClearSelected();
        CurrentRecipe = recipe;
        print("Selected Recipe " + recipe.Name);

        UIDescription.GetComponent<TextMeshProUGUI>().text = CurrentRecipe.Description;
        for (int i = 0; i < CurrentRecipe.Ingridients.Count(); i++)
        {
            GameObject instance = Instantiate(IngridientUIPrefab, Vector3.zero, Quaternion.identity, UIIngridientList.transform);

            RectTransform ui = instance.GetComponent<RectTransform>();

            ui.anchoredPosition3D = StartingPosIngridient + Increment * i;
            ui.localEulerAngles = Vector3.zero;

            instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = CurrentRecipe.Ingridients[i].item.itemName + " x"+ CurrentRecipe.Ingridients[i].count;
        }
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