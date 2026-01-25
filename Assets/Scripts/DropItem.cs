using UnityEngine;
using UnityEngine.UIElements;

public class DropItem : MonoBehaviour
{
    public Item Item;
    public int Amount = 1;

    void Start()
    {
        // Не реализовано
        // if (Item.DropModel != null)
        //     InitModel();
    }

    void InitModel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;

        GameObject model = Instantiate(Item.DropModel, transform);
        model.layer = gameObject.layer;
        Collider test = model.GetComponent<Collider>();
        gameObject.AddComponent<Collider>();
    }

    public void Pickup()
    {
        Amount = PlayerInventory.Instance.AddItem(Item, Amount);

        if (Amount == 0) Destroy(gameObject);
    }
}
