using UnityEngine;
using UnityEngine.UIElements;

public class DropItem : MonoBehaviour
{
    public Item Item;
    public int Amount = 1;
    public bool DisableInstead = false;
    void Start()
    {
        // Не реализовано
        // if (Item.DropModel != null)
        //     InitModel();
    }

    void InitModel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject model = Instantiate(Item.DropModel, transform);
        Collider newCol = model.GetComponent<Collider>();
        
        Collider current = GetComponent<Collider>();
        newCol.includeLayers = current.includeLayers;
        newCol.excludeLayers = current.excludeLayers;

        Destroy( current );
        

        model.layer = gameObject.layer;
        
        
        gameObject.AddComponent<Collider>();
    }

    public void Pickup()
    {
        Amount = PlayerInventory.Instance.AddItem(Item, Amount);

        if (Amount == 0)
        {
            if (DisableInstead)
                Destroy(this);
            else
                Destroy(gameObject);
        }
    }
}
