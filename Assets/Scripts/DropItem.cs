using UnityEngine;
using UnityEngine.UIElements;

public class DropItem : MonoBehaviour
{
    public Item Item;
    public int Amount = 1;


    void InitModel()
    {
        transform.GetChild(0).gameObject.SetActive(false);

        Instantiate(Item.DropModel, transform);
    }

    public void Pickup()
    {
        Amount = PlayerInventory.Instance.AddItem(Item, Amount);

        if (Amount == 0) Destroy(gameObject);
    }
}
